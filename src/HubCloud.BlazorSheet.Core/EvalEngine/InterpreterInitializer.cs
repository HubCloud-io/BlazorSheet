using System;
using DynamicExpresso;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.Core.EvalEngine
{
    public class InterpreterInitializer
    {
        private static SheetData _cells;

        public static Interpreter CreateInterpreter(SheetData cells)
        {
            _cells = cells;

            var interpreter = new Interpreter(InterpreterOptions.DefaultCaseInsensitive);

            interpreter.Reference(typeof(DateTime));
            interpreter.Reference(typeof(MidpointRounding));
            interpreter.Reference(typeof(System.Linq.Enumerable));

            Func<string, object> sumFunction = Sum;
            interpreter.SetFunction(DictMain.SumFunc, sumFunction);
            
            return interpreter;
        }

        private static object Sum(string address)
        {
            var sum = _cells.Sum(address);
            return sum;
        }
    }
}