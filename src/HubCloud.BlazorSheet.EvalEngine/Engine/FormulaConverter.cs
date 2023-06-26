using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.EvalEngine.Engine
{
    public class FormulaConverter
    {
        public static string PrepareFormula(string formulaIn, string contextName)
        {
            var funcDict = new Dictionary<string, string>();
            var sb = new StringBuilder(formulaIn.Replace("$c", contextName));
            
            CutFunctions(sb, funcDict, contextName);
            
            sb = SetValues(sb);
            
            PasteFunctions(sb, funcDict);
            
            var formula = sb.ToString();
            formula = formula
                .Replace("=","==")
                .Replace("====","==")
                .Replace("<>","!=")
                .Replace("!==","!=")
                .Replace(">==",">=")
                .Replace("<==","<=")
                .Replace("==>","=>")
                .Replace(" and "," && ")
                .Replace(" AND "," && ")
                .Replace(" or "," || ")
                .Replace(" OR "," || ");

            return formula;
        }
        
        private static void PasteFunctions(StringBuilder sb, Dictionary<string, string> funcDict)
        {
            foreach (var item in funcDict.Reverse())
            {
                sb = sb.Replace(item.Key, item.Value);
            }
        }

        private static void CutFunctions(StringBuilder sb, Dictionary<string, string> funcDict, string contextName)
        {
            // find functions by format 'FunctionName(..)'
            var regex = new Regex(@"[A-z]+[(][^()]+[)]");
            var functions = regex.Matches(sb.ToString())
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToList();

            var currentIndent = 0;
            foreach (var function in functions)
            {
                var processed = ProcessFunction(function, contextName);
                var key = "{{" + currentIndent++ + "}}";
                sb = sb.Replace(processed, key);
                funcDict.Add(key, processed);
            }
        }

        private static string ProcessFunction(string function, string contextName)
        {
            var startIndex = function.IndexOf("(", StringComparison.InvariantCulture);
            var endIndex = function.IndexOf(")", StringComparison.InvariantCulture);

            var parameters = function.Substring(startIndex + 1, endIndex - startIndex - 1)
                .Split(',')
                .ToList();

            var values = new List<string>();
            foreach (var parameter in parameters)
            {
                if (!parameter.Contains(':'))
                    values.Add(PrepareFormula(parameter, contextName));
                else
                    values.Add(parameter);
            }

            var restoredFunction = function.Substring(0, startIndex + 1);
            foreach (var value in values)
            {
                restoredFunction += $"{value},";
            }

            restoredFunction = restoredFunction.TrimEnd(',');
            restoredFunction += function.Substring(endIndex, function.Length - endIndex);

            return restoredFunction;
        }

        private static StringBuilder SetValues(StringBuilder sb)
        {
            var valueDict = GetValueDict(sb);
            foreach (var item in valueDict)
            {
                sb = sb.Replace(item.Key, item.Value);
            }

            return sb;
        }

        private static Dictionary<string, string> GetValueDict(StringBuilder formula)
            => GetValueDict(formula.ToString());

        private static Dictionary<string, string> GetValueDict(string formula)
        {
            // find cell address by format 'R<some digits>C<some digits>'
            var regex = new Regex(@"R-*\d*C-*\d*");
            var matches = regex.Matches(formula);

            var dict = new Dictionary<string, string>();
            foreach (var match in matches)
            {
                var key = match.ToString();
                var val = $"_cells.GetValue(\"{key}\")";
                dict.Add(key, val);
            }

            return dict;
        }
    }
}