using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DynamicExpresso;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Abstract;
using HubCloud.BlazorSheet.EvalEngine.Engine.DependencyAnalyzer;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HubCloud.BlazorSheet.EvalEngine.Engine
{
    public class WorkbookEvaluator
    {
        private const string ContextName = "_data";

        private readonly IEvaluatorLogger _logger;
        private readonly Interpreter _interpreter;
        private readonly WorkbookData _data;
        private readonly Workbook _workbook;

        private readonly SheetDependencyAnalyzer _analyzer;

        public IEnumerable<IEvaluatorLogMessage> Messages => _logger.Messages;
        public LogLevel LogLevel => _logger.MinimumLevel;

        public WorkbookEvaluator(Workbook workbook)
        {
            _logger = new EvaluatorLogger();

            _data = new WorkbookData(workbook);
            _workbook = workbook;

            _analyzer = new SheetDependencyAnalyzer(_workbook.FirstSheet);

            _interpreter = InterpreterInitializer.CreateInterpreter(_data);
            _interpreter.SetVariable("_data", _data);
        }

        public WorkbookEvaluator(WorkbookData data)
        {
            _logger = new EvaluatorLogger();
            _data = data;

            _interpreter = InterpreterInitializer.CreateInterpreter(_data);
            _interpreter.SetVariable(ContextName, _data);
        }

        public void SetVariable(string name, object data)
        {
            _interpreter.SetVariable(name, data);
        }

        public object Eval(string expression, int row, int column)
        {
            object result = null;
            _data.CurrentRow = row;
            _data.CurrentColumn = column;

            var exceptionList = new List<string> {"SUM"};
            var formulaProcessor = new SimpleFormulaProcessor(exceptionList);

            var formula = string.Empty;
            try
            {
                formula = formulaProcessor.PrepareFormula(expression, ContextName);
                result = _interpreter.Eval(formula);

                _logger.LogDebug("Cell:R{0}C{1}. Formula: {2}. Result: {3}."
                    , row
                    , column
                    , formula
                    , result);
            }
            catch (Exception e)
            {
                _logger.LogError("Cell:R{0}C{1}. Cannot eval Formula: {2}. Prepared formula: {3}. Message: {4}"
                    , row
                    , column
                    , expression
                    , formula
                    , e.Message);
            }

            return result;
        }

        public void EvalWorkbook()
        {
            foreach (var sheet in _workbook.Sheets)
            {
                var cells = _data.GetSheetByName(sheet.Name);
                EvalSheet(sheet, cells);
            }
        }

        public void EvalWorkbook(SheetCellAddress cellAddress)
        {
            var sheet = _workbook.FirstSheet;
            var cells = _data.GetSheetByName(sheet.Name);
            EvalSheet(sheet, cells, cellAddress);
        }

        // for recalc only dependent formula by cell 
        private void EvalSheet(Sheet sheet, SheetData cells, SheetCellAddress cellAddress)
        {
            // var analyzer = new SheetDependencyAnalyzer(sheet);
            // var dependencyCells = analyzer.GetDependencyCells(cellAddress);

            try
            {
                var dependencyCells2 = _analyzer.GetDependencyCells2(cellAddress);
                foreach (var cell in dependencyCells2)
                {
                    EvalCell(cell, sheet, cells);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        // for recalc full sheet
        private void EvalSheet(Sheet sheet, SheetData cells)
        {
            //var formulaCells = sheet.Cells
            //    .Where(x => !string.IsNullOrWhiteSpace(x.Formula))
            //    .ToList();

            //var valueCells = SheetDependencyAnalyzer.GetNoFormulaCells(sheet);
            //var nonNullValueCells = sheet.Cells
            //    .Where(x => x.Value != null)
            //    .Select(x => SheetDependencyAnalyzer.GetCellAddress(sheet.CellAddress(x)));

            //valueCells.AddRange(nonNullValueCells);

            //var dict = formulaCells
            //    .ToDictionary(k => SheetDependencyAnalyzer.GetCellAddress(sheet.CellAddress(k)), v => v);

            //var dependencyCells = _analyzer.OrderCellsForCalc(valueCells, dict);
            try
            {
                var dependencyCells = _analyzer.OrderCellsForCalc2();
                foreach (var cell in dependencyCells)
                {
                    EvalCell(cell, sheet, cells);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private void EvalCell(SheetCell cell, Sheet sheet, SheetData cells)
        {
            var cellAddress = sheet.CellAddressSlim(cell);
            var evalResult = Eval(cell.Formula, cellAddress.Row, cellAddress.Column);

            if (evalResult is UniversalValue uValue)
                cell.Value = uValue.Value;
            else
                cell.Value = evalResult;

            var editSettings = sheet.GetEditSettings(cell);
            cell.ApplyFormat(editSettings.CellDataType);

            cells[cellAddress.Row, cellAddress.Column] = cell.Value;
        }

        public void SetValue(int row, int column, object value)
        {
            var cells = _data.FirstSheet;

            cells[row, column] = value;
        }


        public void SetLogLevel(LogLevel level)
        {
            _logger.MinimumLevel = level;
        }

        public void ClearLog()
        {
            _logger.Clear();
        }
    }
}