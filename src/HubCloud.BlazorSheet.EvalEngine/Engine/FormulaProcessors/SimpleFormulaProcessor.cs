using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors
{
    public class SimpleFormulaProcessor
    {
        private List<string> _exceptionList = new List<string>
        {
            "VAL"
        };

        public SimpleFormulaProcessor(List<string> exceptionFunctionsList = null)
        {
            if (exceptionFunctionsList is null)
                return;

            foreach (var funcName in exceptionFunctionsList)
            {
                if (!_exceptionList.Any(x => x.Trim().ToUpper() == funcName.Trim().ToUpper()))
                    _exceptionList.Add(funcName);
            }
        }

        public string PrepareFormula(string formulaIn, string contextName)
        {
            var formula = new StringBuilder(formulaIn)
                .Replace("$c", contextName)
                .Replace("=", "==")
                .Replace("====", "==")
                .Replace("<>", "!=")
                .Replace("!==", "!=")
                .Replace(">==", ">=")
                .Replace("<==", "<=")
                .Replace("==>", "=>")
                .Replace(" and ", " && ")
                .Replace(" AND ", " && ")
                .Replace(" or ", " || ")
                .Replace(" OR ", " || ");
            
            // prepare formula
            var exceptionDict = GetExceptionDict(formula);
            var addressRangeDict = GetAddressRangeDict(formula);
            var addressDict = GetAddressDict(formula);
            
            // process
            foreach (var address in addressDict)
            {
                addressDict[address.Key] = $@"VAL(""{address.Value.Trim('\"')}"")";
            }

            // build formula
            ReplaceFromDict(formula, addressDict);
            ReplaceFromDict(formula, addressRangeDict);
            ReplaceFromDict(formula, exceptionDict);

            return formula.ToString();
        }

        private void ReplaceFromDict(StringBuilder formula, Dictionary<string, string> dict)
        {
            foreach (var item in dict)
                formula.Replace(item.Key, item.Value);
        }

        private Dictionary<string, string> GetAddressRangeDict(StringBuilder formula)
        {
            var dict = new Dictionary<string, string>();
            var regex = new Regex(@"R-*\d*C-*\d*:R-*\d*C-*\d*");
            var matches = regex.Matches(formula.ToString())
                .Cast<Match>()
                .Select(x => x.Value)
                .Distinct();

            var i = 0;
            foreach (var item in matches)
            {
                var key = "{AR" + i++ + "}";
                dict.Add(key, item);
                formula.Replace(item, key);
            }

            return dict;
        }

        private Dictionary<string, string> GetExceptionDict(StringBuilder formula)
        {
            var dict = new Dictionary<string, string>();
            if (_exceptionList == null || _exceptionList.Count == 0)
                return dict;

            var i = 0;
            foreach (var item in _exceptionList)
            {
                do
                {
                    var index = formula.ToString().IndexOf(item, StringComparison.InvariantCultureIgnoreCase);
                    if (index == -1)
                        break;

                    var startIndex = index;
                    index += item.Length;
                    while (index < formula.Length && formula[index] != '(')
                    {
                        index++;
                    }
                    
                    var balance = 0;
                    while (index < formula.Length)
                    {
                        if (formula[index] == '(')
                            balance++;
                        if (formula[index] == ')')
                            balance--;

                        if (formula[index] == ')' && balance == 0)
                            break;
                        
                        index++;
                    }

                    var endIndex = index;

                    var key = "{EX" + i + "}";
                    var exceptionStatement = formula.ToString().Substring(startIndex, endIndex - startIndex + 1);
                    formula.Replace(exceptionStatement, key, startIndex, endIndex - startIndex + 1);
                    
                    dict.Add(key, exceptionStatement);
                    
                    i++;

                } while (true);
            }
            
            return dict;
        }

        private Dictionary<string, string> GetAddressDict(StringBuilder formula)
        {
            var regex = new Regex(@"""*R-*\d*C-*\d*""*");
            var matches = regex.Matches(formula.ToString())
                .Cast<Match>()
                .Select(x => x.Value)
                .Distinct();

            var addressDict = new Dictionary<string, string>();
            var i = 0;
            foreach (var match in matches)
            {
                var key = "{" + i++ + "}";
                addressDict.Add(key, match);
                formula.Replace(match, key);
            }

            return addressDict;
        }
    }
}