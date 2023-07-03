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
            interpreter.Reference(typeof(MidpointRounding));
            interpreter.Reference(typeof(System.Linq.Enumerable));

            Func<string, UniversalValue> sumFunction = data.Sum;
            interpreter.SetFunction("SUM", sumFunction);
            
            Func<string, UniversalValue> valFunction = data.GetValue;
            interpreter.SetFunction("VAL", valFunction);
            
            Func<UniversalValue> rowFunction = data.Row;
            interpreter.SetFunction("ROW", rowFunction);
            
            Func<UniversalValue> columnFunction = data.Column;
            interpreter.SetFunction("COLUMN", columnFunction);

            Func<UniversalValue, UniversalValue> isEmptyFunction = data.IsEmpty;
            interpreter.SetFunction("IsEmpty", isEmptyFunction);

            Func<UniversalValue, UniversalValue> isNotEmptyFunction = data.IsNotEmpty;
            interpreter.SetFunction("IsNotEmpty", isNotEmptyFunction);

            Func<UniversalValue> nowFunction = data.Now;
            interpreter.SetFunction("NOW", nowFunction);
            
            Func<string, UniversalValue, UniversalValue, UniversalValue> dateDiffFunction = data.DateDiff;
            interpreter.SetFunction("DateDiff", dateDiffFunction);

            Func<bool, object, object, UniversalValue> iifFunction = data.Iif;
            interpreter.SetFunction("iif", iifFunction);

            Func<object[], UniversalValue> ifsFunction = data.Ifs;
            interpreter.SetFunction("ifs", ifsFunction);

            return interpreter;
        }
    }
}