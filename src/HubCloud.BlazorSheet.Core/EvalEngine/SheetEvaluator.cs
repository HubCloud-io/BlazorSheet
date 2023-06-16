using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DynamicExpresso;
using DynamicExpresso.Exceptions;
using HubCloud.BlazorSheet.Core.EvalEngine.Abstract;
using HubCloud.BlazorSheet.Core.Models;
using Microsoft.Extensions.Logging;

namespace HubCloud.BlazorSheet.Core.EvalEngine
{
    public class SheetEvaluator
    {
        private const string ContextName = "_cells";
        
        private readonly IEvaluatorLogger _logger;
        private Interpreter _interpreter;
        private SheetData _cells;
        private Sheet _sheet;
        
        public IEnumerable<IEvaluatorLogMessage> Messages => _logger.Messages;
        public LogLevel LogLevel => _logger.MinimumLevel;
        
        public SheetEvaluator(Sheet sheet)
        {
            _cells = new SheetData(sheet);
            _sheet = sheet;
            _logger = new EvaluatorLogger();
            
            _interpreter = InterpreterInitializer.CreateInterpreter(_cells);
            _interpreter.SetVariable(ContextName, _cells);
        }

        public SheetEvaluator(SheetData cells)
        {
            _cells = cells;
            _logger = new EvaluatorLogger();
            
            _interpreter = InterpreterInitializer.CreateInterpreter(_cells);
            _interpreter.SetVariable(ContextName, _cells);
        }

        public void SetValue(int row, int column, object value)
        {
            _cells[row, column] =  value;
        }

        public object Eval(string expression, int row, int column)
        {
            object result = null;

            var formulaConverter = new FormulaConverter();
            var formula = formulaConverter.PrepareFormula(expression, ContextName);

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

        public void EvalSheet()
        {
            var formulaCells = _sheet.Cells.Where(x => !string.IsNullOrWhiteSpace(x.Formula)).ToList();
            foreach (var cell in formulaCells)
            {
                var cellAddress = _sheet.CellAddress(cell);
                _cells.CurrentRow = cellAddress.Row;
                _cells.CurrentColumn = cellAddress.Column;
                
                var evalResult = Eval(cell.Formula, cellAddress.Row, cellAddress.Column);

                if (evalResult is UniversalValue uValue)
                    cell.Value = uValue.Value;
                else
                    cell.Value = evalResult;
               
                cell.Text = cell.Value?.ToString();
                
                _cells[cellAddress.Row, cellAddress.Column] = cell.Value;
            }
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