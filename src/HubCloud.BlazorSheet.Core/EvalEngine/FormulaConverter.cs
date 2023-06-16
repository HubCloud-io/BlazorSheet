﻿using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.Core.EvalEngine
{
    public class FormulaConverter : IFormulaConverter
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
                isFound = IsolateFunctions(sb, funcDict);
            } while (isFound);

            sb = SetValues(sb);

            foreach (var item in funcDict.Reverse())
            {
                sb = sb.Replace(item.Key, item.Value);
            }

            return sb.ToString();
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

        private bool IsolateFunctions(StringBuilder sb, Dictionary<string, string> funcDict)
        {
            var regex = new Regex(@"[A-z]+[(][^()]+[)]");
            var functions = regex.Matches(sb.ToString())
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToList();

            var processedFunctions = new List<string>();
            foreach (var function in functions)
            {
                processedFunctions.Add(ProcessFunction(function));
            }

            foreach (var function in processedFunctions)
            {
                var key = "{{" + _currentIndent++ + "}}";
                sb = sb.Replace(function, key);
                funcDict.Add(key, function);
            }

            return functions.Any();
        }

        private string ProcessFunction(string function)
        {
            var startIndex = function.IndexOf("(", StringComparison.InvariantCulture);
            var endIndex = function.IndexOf(")", StringComparison.InvariantCulture);

            var parameters = function.Substring(startIndex + 1, endIndex - startIndex - 1)
                .Split(',')
                .Where(x => !x.Contains(':'))
                .ToList();

            var parameterValues = new List<string>();
            foreach (var parameter in parameters)
            {
                var v = SetValues(new StringBuilder(parameter)).ToString();
                parameterValues.Add(v);
            }

            var restoredFunction = function.Substring(0, startIndex + 1);
            foreach (var value in parameterValues)
            {
                restoredFunction += $"{value},";
            }

            restoredFunction = restoredFunction.TrimEnd(',');
            restoredFunction += function.Substring(endIndex, function.Length - endIndex);

            return restoredFunction;
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