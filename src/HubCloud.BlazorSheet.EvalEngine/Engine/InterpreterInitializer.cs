using System;
using DynamicExpresso;

namespace HubCloud.BlazorSheet.EvalEngine.Engine
{
    public class InterpreterInitializer
    {
        public static Interpreter CreateInterpreter()
        {
            var interpreter = new Interpreter(InterpreterOptions.DefaultCaseInsensitive);

            interpreter.Reference(typeof(DateTime));
            interpreter.Reference(typeof(System.MidpointRounding));
            interpreter.Reference(typeof(System.Linq.Enumerable));

            return interpreter;
        }
    }
}