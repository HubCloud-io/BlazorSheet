using System;
using DynamicExpresso;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine
{
    public class InterpreterInitializer
    {
        
        private static WorkbookData _data;
        
        public static Interpreter CreateInterpreter(WorkbookData data)
        {
            _data = data;
            
            var interpreter = new Interpreter(InterpreterOptions.DefaultCaseInsensitive);

            interpreter.Reference(typeof(DateTime));
            interpreter.Reference(typeof(System.MidpointRounding));
            interpreter.Reference(typeof(System.Linq.Enumerable));

            Func<string, UniversalValue> sumFunction = Sum;
            interpreter.SetFunction("SUM", sumFunction);
            
            Func<string, UniversalValue> valFunction = Val;
            interpreter.SetFunction("VAL", valFunction);
            
            return interpreter;
        }
        
        private static UniversalValue Sum(string address)
        {
            var result = _data.Sum(address);
            return result;
        }
        
        private static UniversalValue Val(string address)
        {
            var result = _data.GetValue(address);
            return result;
        }
        
    }
}