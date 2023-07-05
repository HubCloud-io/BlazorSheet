using System;
using System.Collections.Generic;
using System.Linq;
using DynamicExpresso;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Abstract;
using HubCloud.BlazorSheet.EvalEngine.Engine.DependencyAnalyzer;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaHelpers;
using Microsoft.Extensions.Logging;

namespace HubCloud.BlazorSheet.EvalEngine.Engine
{
    public class WorkbookEvaluator
    {
        private const string ContextName = "_data";
        
        private readonly IEvaluatorLogger _logger;
        private readonly Interpreter _interpreter;
        private readonly WorkbookData _data;
        private readonly Workbook _workbook;
        
        public IEnumerable<IEvaluatorLogMessage> Messages => _logger.Messages;
        public LogLevel LogLevel => _logger.MinimumLevel;

        public WorkbookEvaluator(Workbook workbook)
        {
            _logger = new EvaluatorLogger();
            
            _data = new WorkbookData(workbook);
            _workbook = workbook;
            
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
        
        public object Eval(string expression, int row, int column)
        {
            object result = null;
            _data.CurrentRow = row;
            _data.CurrentColumn = column;

            var converter = new FormulaProcessor();
            var exceptionList = new List<string> { "SUM" };
            
            var formula = converter.PrepareFormula(expression, ContextName, exceptionList);

            try
            {
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
                    ,row
                    ,column
                    ,expression
                    ,formula
                    ,e.Message);

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

        private void EvalSheet(Sheet sheet, SheetData cells, SheetCellAddress cellAddress)
        {
            var analyzer = new SheetDependencyAnalyzer(sheet);
            var dependencyCells = analyzer.GetDependencyCells(cellAddress);
            
            foreach (var cell in dependencyCells)
            {
                EvalCell(cell, sheet, cells);
            }
        }

        private void EvalSheet(Sheet sheet, SheetData cells)
        {
            var formulaCells = sheet.Cells
                .Where(x => !string.IsNullOrWhiteSpace(x.Formula))
                .ToList();

            var valueCells = SheetDependencyAnalyzer.GetNullValueCellAddresses(sheet);
            var nonNullValueCells = sheet.Cells
                .Where(x => x.Value != null)
                .Select(x => SheetDependencyAnalyzer.GetCellAddress(sheet.CellAddress(x)));
            
            valueCells.AddRange(nonNullValueCells);

            var dict = formulaCells
                .ToDictionary(k => SheetDependencyAnalyzer.GetCellAddress(sheet.CellAddress(k)), v => v);
            
            var analyzer = new SheetDependencyAnalyzer(sheet);
            var dependencyCells = analyzer.OrderCellsForCalc(valueCells, dict);
            
            foreach (var cell in dependencyCells)
            {
                EvalCell(cell, sheet, cells);
            }
        }

        private void EvalCell(SheetCell cell, Sheet sheet, SheetData cells)
        {
            var cellAddress = sheet.CellAddress(cell);
            var evalResult = Eval(cell.Formula, cellAddress.Row, cellAddress.Column);
            
            if (evalResult is UniversalValue uValue)
                cell.Value = uValue.Value;
            else
                cell.Value = evalResult;
               
            cell.Text = cell.Value?.ToString();
            cells[cellAddress.Row, cellAddress.Column] = cell.Value;
        }
        
        public void SetValue(int row, int column, object value)
        {

            var cells = _data.FirstSheet;
            
            cells[row, column] =  value;
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