using System;
using System.Collections.Generic;
using System.Linq;
using DynamicExpresso;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Abstract;
using Microsoft.Extensions.Logging;

namespace HubCloud.BlazorSheet.EvalEngine.Engine
{
    public class SheetEvaluator
    {
        private readonly IEvaluatorLogger _logger;
        private Interpreter _interpreter;
        private SheetData _cells;
        private Sheet _sheet;
        
        public IEnumerable<IEvaluatorLogMessage> Messages => _logger.Messages;
        public LogLevel LogLevel => _logger.MinimumLevel;

        public SheetEvaluator(Sheet sheet)
        {
            _logger = new EvaluatorLogger();
           // _interpreter = InterpreterInitializer.CreateInterpreter();

            _cells = new SheetData(sheet);
            _sheet = sheet;
            
            _interpreter.SetVariable("_cells", _cells);
        }

        public SheetEvaluator(SheetData cells)
        {
            _logger = new EvaluatorLogger();
          //  _interpreter = InterpreterInitializer.CreateInterpreter();
            
            _cells = cells;
            _interpreter.SetVariable("_cells", _cells);
        }

        public void SetValue(int row, int column, object value)
        {
            _cells[row, column] =  value;
        }
        
        public object Eval(string expression, int row, int column)
        {
            object result = null;

            _interpreter.SetVariable("_currentRow", row);
            _interpreter.SetVariable("_currentColumn", column);

            var formula = FormulaConverter.PrepareFormula(expression);

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
                _logger.LogError("Cell:R{0}C{1}. Cannot eval Formula: {0}. Prepared formula: {1}. Message: {2}"
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
            foreach (var cell in _sheet.Cells.Where(x=>!string.IsNullOrWhiteSpace(x.Formula)))
            {
                var cellAddress = _sheet.CellAddress(cell);
                _cells.CurrentRow = cellAddress.Row;
                _cells.CurrentColumn = cellAddress.Column;
                
                var evalResult = Eval(cell.Formula, cellAddress.Row, cellAddress.Column);

                if (evalResult is UniversalValue uValue)
                {
                    cell.Value = uValue.Value;
                }
                else
                {
                    cell.Value = evalResult;
                }
               
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