using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.EvalEngine.Engine
{
    // public class FormulaConverter
    // {
    //     private const string ValFunctionName = "VAL";
    //
    //     private static List<string> _exceptionList = new List<string>
    //     {
    //         ValFunctionName
    //     };
    //
    //     public static string PrepareFormula(string formulaIn, string contextName)
    //     {
    //         var funcDict = new Dictionary<string, string>();
    //         var sb = new StringBuilder(formulaIn.Replace("$c", contextName));
    //
    //         CutFunctions(sb, funcDict, contextName);
    //         
    //         sb = ProcessValues(sb, contextName);
    //         
    //         PasteFunctions(sb, funcDict);
    //
    //         var formula = sb.ToString();
    //         formula = formula
    //             .Replace("=", "==")
    //             .Replace("====", "==")
    //             .Replace("<>", "!=")
    //             .Replace("!==", "!=")
    //             .Replace(">==", ">=")
    //             .Replace("<==", "<=")
    //             .Replace("==>", "=>")
    //             .Replace(" and ", " && ")
    //             .Replace(" AND ", " && ")
    //             .Replace(" or ", " || ")
    //             .Replace(" OR ", " || ");
    //
    //         return formula;
    //     }
    //
    //     public static void AddToExceptionList(string functionName)
    //     {
    //         if (_exceptionList.Any(x => x == functionName.ToUpper()))
    //             return;
    //
    //         _exceptionList.Add(functionName.ToUpper());
    //     }
    //
    //     private static void CutFunctions(StringBuilder sb, Dictionary<string, string> funcDict, string contextName)
    //     {
    //         // find functions by format 'FunctionName(..)'
    //         var regex = new Regex(@"[A-z]+[(][^()]+[)]");
    //         var functions = regex.Matches(sb.ToString())
    //             .Cast<Match>()
    //             .Select(m => m.Value)
    //             .Distinct()
    //             .ToList();
    //
    //         var currentIndent = 0;
    //         // foreach (var function in functions.Where(x => !_exceptionList.Contains(GetFunctionName(x))))
    //         foreach (var function in functions)
    //         {
    //             string processed;
    //             if (!_exceptionList.Contains(GetFunctionName(function)))
    //                 processed = ProcessFunction(function, contextName);
    //             else
    //                 processed = ProcessExceptionFunction(function, contextName);
    //             
    //             var key = "{{" + currentIndent++ + "}}";
    //             sb = sb.Replace(function, key);
    //             funcDict.Add(key, processed);
    //         }
    //
    //         // foreach (var function in functions.Where(x => _exceptionList.Contains(GetFunctionName(x))))
    //         // {
    //         //     var processed = ProcessExceptionFunction(function, contextName);
    //         //     var key = "{{" + currentIndent++ + "}}";
    //         //     sb = sb.Replace(function, key);
    //         //     funcDict.Add(key, processed);
    //         // }
    //     }
    //
    //     private static void PasteFunctions(StringBuilder sb, Dictionary<string, string> funcDict)
    //     {
    //         foreach (var item in funcDict.Reverse())
    //         {
    //             sb = sb.Replace(item.Key, item.Value);
    //         }
    //     }
    //
    //     private static string ProcessExceptionFunction(string function, string contextName)
    //     {
    //         if (GetFunctionName(function).ToUpper() != ValFunctionName)
    //             return function;
    //
    //         var param = GetFunctionParams(function).Aggregate((x, y) => $"{x}, {y}");
    //         var val = $"{contextName}.GetValue(\"{param}\")";
    //         return val;
    //     }
    //
    //     private static string ProcessFunction(string function, string contextName)
    //     {
    //         var startIndex = function.IndexOf("(", StringComparison.InvariantCulture);
    //         var endIndex = function.IndexOf(")", StringComparison.InvariantCulture);
    //
    //         var parameters = GetFunctionParams(function);
    //
    //         var values = new List<string>();
    //         foreach (var parameter in parameters)
    //         {
    //             values.Add(PrepareFormula(parameter, contextName));
    //         }
    //
    //         var restoredFunction = function.Substring(0, startIndex + 1);
    //         foreach (var value in values)
    //         {
    //             restoredFunction += $"{value}, ";
    //         }
    //
    //         restoredFunction = restoredFunction.Trim().TrimEnd(',');
    //         restoredFunction += function.Substring(endIndex, function.Length - endIndex);
    //
    //         return restoredFunction;
    //     }
    //
    //     private static StringBuilder ProcessValues(StringBuilder sb, string contextName)
    //     {
    //         var valueDict = GetValueDict(sb, contextName);
    //         foreach (var item in valueDict)
    //         {
    //             sb = sb.Replace(item.Key, item.Value);
    //         }
    //
    //         return sb;
    //     }
    //
    //     private static Dictionary<string, string> GetValueDict(StringBuilder formula, string contextName)
    //         => GetValueDict(formula.ToString(), contextName);
    //
    //     private static Dictionary<string, string> GetValueDict(string formula, string contextName)
    //     {
    //         // find cell address by format 'R<some digits>C<some digits>'
    //         var regex = new Regex(@"R-*\d*C-*\d*");
    //         var matches = regex.Matches(formula);
    //
    //         var dict = new Dictionary<string, string>();
    //         foreach (var match in matches)
    //         {
    //             var key = match.ToString();
    //             var val = $"{contextName}.GetValue(\"{key}\")";
    //             dict.Add(key, val);
    //         }
    //
    //         return dict;
    //     }
    //
    //     private static List<string> GetFunctionParams(string function)
    //     {
    //         var startIndex = function.IndexOf("(", StringComparison.InvariantCulture);
    //         var endIndex = function.IndexOf(")", StringComparison.InvariantCulture);
    //
    //         var parameters = function.Substring(startIndex + 1, endIndex - startIndex - 1)
    //             .Split(',')
    //             .Select(x => x.Trim().Replace(@"""", ""))
    //             .ToList();
    //
    //         return parameters;
    //     }
    //
    //     private static string GetFunctionName(string function)
    //     {
    //         var startIndex = function.IndexOf("(", StringComparison.InvariantCulture);
    //         var functionName = function.Substring(0, startIndex);
    //
    //         return functionName;
    //     }
    // }
}