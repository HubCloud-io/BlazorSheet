using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.Core.EvalEngine
{
    public class FormulaConverter2 : IFormulaConverter
    {
        private string _contextName;
        public string PrepareFormula(string formula, string contextName)
        {
            _contextName = contextName;
            return PrepareFormulaInner(formula);
        }

        private string PrepareFormulaInner(string formula)
        {
            var funcDict = new Dictionary<string, string>();
            var sb = new StringBuilder(formula.Replace("$c", _contextName));
            
            CutFunctions(sb, funcDict);
            
            sb = SetValues(sb);

            PasteFunctions(sb, funcDict);

            return sb.ToString();
        }

        private void PasteFunctions(StringBuilder sb, Dictionary<string, string> funcDict)
        {
            foreach (var item in funcDict.Reverse())
            {
                sb = sb.Replace(item.Key, item.Value);
            }
        }
        
        private void CutFunctions(StringBuilder sb, Dictionary<string, string> funcDict)
        {
            var regex = new Regex(@"[A-z]+[(][^()]+[)]");
            var functions = regex.Matches(sb.ToString())
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToList();

            var currentIndent = 0;
            foreach (var function in functions)
            {
                var processed = ProcessFunction(function);
                var key = "{{" + currentIndent++ + "}}";
                sb = sb.Replace(processed, key);
                funcDict.Add(key, processed);
            }
        }

        private string ProcessFunction(string function)
        {
            var startIndex = function.IndexOf("(", StringComparison.InvariantCulture);
            var endIndex = function.IndexOf(")", StringComparison.InvariantCulture);
            
            var parameters = function.Substring(startIndex + 1, endIndex - startIndex - 1)
                .Split(',')
                .ToList();

            // var values = new List<string>();
            // foreach (var parameter in parameters.Where(x => !x.Contains(':')))
            // {
            //     values.Add(PrepareFormulaInner(parameter));
            // }
            
            var restoredFunction = function.Substring(0, startIndex + 1);
            foreach (var value in parameters)
            {
                restoredFunction += $"{value},";
            }

            restoredFunction = restoredFunction.TrimEnd(',');
            restoredFunction += function.Substring(endIndex, function.Length - endIndex);
            
            return restoredFunction;
        }

        private StringBuilder SetValues(StringBuilder sb)
        {
            var valueDict = GetValueDict(sb);
            foreach (var item in valueDict)
            {
                sb = sb.Replace(item.Key, item.Value);
            }

            return sb;
        }
        
        private Dictionary<string, string> GetValueDict(StringBuilder formula)
            => GetValueDict(formula.ToString());
        
        private Dictionary<string, string> GetValueDict(string formula)
        {
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