using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.Core.EvalEngine
{
    public class FormulaConverter
    {
        public static string PrepareFormula(string formula, string contextName)
        {
            var sb = new StringBuilder(formula.Replace("$c", contextName));
            
            var regex = new Regex(@"[A-z]+[(][^()]+[)]");
            var matches = regex.Matches(formula)
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToList();

            var i = 0;
            var functionDict = new Dictionary<string, string>();
            foreach (var match in matches)
            {
                var key = "{{" + i++ + "}}";
                sb = sb.Replace(match, key);
                functionDict.Add(key, match);
            }

            var valueDict = GetValueDict(sb);
            foreach (var item in valueDict)
            {
                sb = sb.Replace(item.Key, item.Value);
            }
            
            foreach (var item in functionDict)
            {
                sb = sb.Replace(item.Key, item.Value);
            }

            return sb.ToString();
        }
        
        private static Dictionary<string, string> GetValueDict(StringBuilder formula)
        {
            var regex = new Regex(@"R-*\d*C-*\d*");
            var matches = regex.Matches(formula.ToString());

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