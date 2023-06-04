using System;
using System.Collections.Generic;
using System.Linq;
using DynamicExpresso;
using HubCloud.BlazorSheet.Core.EvalEngine.Abstract;
using HubCloud.BlazorSheet.Core.Models;
using Microsoft.Extensions.Logging;

namespace HubCloud.BlazorSheet.Core.EvalEngine
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
            _interpreter = InterpreterInitializer.CreateInterpreter();

            _cells = new SheetData(sheet);
            _sheet = sheet;
            
            _interpreter.SetVariable("_cells", _cells);
        }

        public SheetEvaluator(SheetData cells)
        {
            _logger = new EvaluatorLogger();
            _interpreter = InterpreterInitializer.CreateInterpreter();
            
            _cells = cells;
            _interpreter.SetVariable("_cells", _cells);
        }

        public void SetValue(int row, int column, object value)
        {
            _cells[row, column] =  value;
        }
        
        public object Eval(string expression)
        {
            object result = null;

            var formula = FormulaConverter.PrepareFormula(expression);

            try
            {
                result = _interpreter.Eval(formula);
                
                  
                _logger.LogDebug("Formula eval: {0}. Result: {1}."
                    , formula
                    , result);
                
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot eval Formula: {0}. Prepared formula: {1}. Message: {2}",
                    expression,
                    formula,
                    e.Message);

            }

            return result;
        }

        public void EvalSheet()
        {
            foreach (var cell in _sheet.Cells.Where(x=>!string.IsNullOrWhiteSpace(x.Formula)))
            {
                var evalResult = Eval(cell.Formula);

                if (evalResult is UniversalValue uValue)
                {
                    cell.Value = uValue.Value;
                }
                else
                {
                    cell.Value = evalResult;
                }
               
                cell.Text = cell.Value?.ToString();

                var cellCoordinates = _sheet.CellCoordinates(cell);
                _cells[cellCoordinates.Item1, cellCoordinates.Item2] = new UniversalValue(cell.Value);
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