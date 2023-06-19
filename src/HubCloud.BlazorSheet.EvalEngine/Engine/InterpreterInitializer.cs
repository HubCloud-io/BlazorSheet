using System;
using DynamicExpresso;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine
{
    public class InterpreterInitializer
    {

        public static Interpreter CreateInterpreter(WorkbookData data)
        {
            
            var interpreter = new Interpreter(InterpreterOptions.DefaultCaseInsensitive);

            interpreter.Reference(typeof(DateTime));
            interpreter.Reference(typeof(System.MidpointRounding));
            interpreter.Reference(typeof(System.Linq.Enumerable));

            Func<string, UniversalValue> sumFunction = data.Sum;
            interpreter.SetFunction("SUM", sumFunction);
            
            Func<string, UniversalValue> valFunction = data.GetValue;
            interpreter.SetFunction("VAL", valFunction);
            
            return interpreter;
        }
        
    
        
    }
}