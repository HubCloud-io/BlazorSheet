using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.Core.EvalEngine
{
    public class FormulaConverter
    {
        private int _currentIndent;
        public string PrepareFormula(string formula, string contextName)
        {
            var funcDict = new Dictionary<string, string>();
            
            formula = formula.Replace("$c", contextName);
            var sb = new StringBuilder(formula);

            _currentIndent = 0;

            bool isFound;
            do
            {
                isFound = Isolate(sb, funcDict);
            } while (isFound);
            
            var valueDict = GetValueDict(sb);
            foreach (var item in valueDict)
            {
                sb = sb.Replace(item.Key, item.Value);
            }

            foreach (var item in funcDict.Reverse())
            {
                sb = sb.Replace(item.Key, item.Value);
            }

            return sb.ToString();
        }

        private bool Isolate(StringBuilder sb, Dictionary<string, string> funcDict)
        {
            var regex = new Regex(@"[A-z]+[(][^()]+[)]");
            var matches = regex.Matches(sb.ToString())
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToList();

            foreach (var match in matches)
            {
                var key = "{{" + _currentIndent++ + "}}";
                sb = sb.Replace(match, key);
                funcDict.Add(key, match);
            }

            return matches.Any();
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