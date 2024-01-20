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

        #region eval full workbook methods
        public void EvalWorkbook()
        {
            foreach (var sheet in _workbook.Sheets)
            {
                var cells = _data.GetSheetByName(sheet.Name);
                // EvalSheet(sheet, cells);
                EvalSheet2(sheet, cells);
            }
        }
        
        private void EvalSheet2(Sheet sheet, SheetData cells)
        {
            var formulaCells = sheet.Cells
                .Where(x => IsFormula(x.Formula))
                .ToList();

            // formulas without depend
            var endingFormulas = new List<ValueAddress>();
            foreach (var formulaCell in formulaCells)
            {
                var valueAddress = sheet.CellAddressSlim(formulaCell);
                if (!_analyzer.GetDependencyFormulaDict().TryGetValue(valueAddress, out _))
                    endingFormulas.Add(valueAddress);
            }

            var evaluatedCells = new HashSet<ValueAddress>();

            EvalSheet2Inner(endingFormulas);

            // inner method for recursion
            void EvalSheet2Inner(IEnumerable<ValueAddress> addresses)
            {
                foreach (var address in addresses)
                {
                    if (!evaluatedCells.Contains(address))
                    {
                        _analyzer.GetFormulaAddressesDict().TryGetValue(address, out var addrList);
                        if (addrList?.Any() == true)
                            EvalSheet2Inner(addrList);
                    }

                    var currentCell = sheet.GetCell(address.Row, address.Column);
                    if (IsFormula(currentCell.Formula) && !evaluatedCells.Contains(address))
                        EvalCell(currentCell, sheet, cells);

                    evaluatedCells.Add(address);
                }
            }
        }
        
        private void EvalSheet(Sheet sheet, SheetData cells)
        {
            // var formulaCells = sheet.Cells
            //     .Where(x => !string.IsNullOrWhiteSpace(x.Formula))
            //     .ToList();
            //
            // var valueCells = SheetDependencyAnalyzer.GetNoFormulaCells(sheet);
            // var nonNullValueCells = sheet.Cells
            //     .Where(x => x.Value != null)
            //     .Select(x => SheetDependencyAnalyzer.GetCellAddress(sheet.CellAddress(x)));
            //
            // valueCells.AddRange(nonNullValueCells);
            //
            // var dict = formulaCells
            //     .ToDictionary(k => SheetDependencyAnalyzer.GetCellAddress(sheet.CellAddress(k)), v => v);
            //
            // var dependencyCells = _analyzer.OrderCellsForCalc(valueCells, dict);
            // foreach (var cell in dependencyCells)
            // {
            //     EvalCell(cell, sheet, cells);
            // }

            try
            {
                var dependencyCells = _analyzer.OrderCellsForCalc2();
                foreach (var cell in dependencyCells)
                {
                    EvalCell(cell, sheet, cells);
                }
                // foreach (var cell in dependencyCells)
                // {
                //     EvalCell(cell, sheet, cells);
                // }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
        #endregion

        #region eval workbook on cell change
        public void EvalWorkbook(SheetCellAddress cellAddress)
        {
            var sheet = _workbook.FirstSheet;
            var cells = _data.GetSheetByName(sheet.Name);
            EvalSheet(sheet, cells, cellAddress);
        }
        
        private void EvalSheet(Sheet sheet, SheetData cells, SheetCellAddress cellAddress)
        {
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
        #endregion

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
        
        private bool IsFormula(string formula)
        {
            return !string.IsNullOrEmpty(formula) &&
                   formula.ToUpper().Contains("R") &&
                   formula.ToUpper().Contains("C");
        }
    }
}