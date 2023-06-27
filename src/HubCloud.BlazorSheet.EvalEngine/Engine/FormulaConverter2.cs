﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.EvalEngine.Engine
{
    public enum ElementType
    {
        Numeric,
        Operator,
        Function,
        Address
    }

    public class Statement
    {
        private List<string> _paramsList = new List<string>();
        
        public ElementType Type { get; set; }
        public string OriginExpression { get; set; }
        public string ProcessedExpression => OriginExpression;
        public string FunctionName { get; set; }
        public string FunctionParams => _paramsList.Aggregate((x, y) => $"{x}, {y}");
        public void SetFunctionParameters(StringBuilder parameters)
        {
            var p = parameters.ToString()
                .Trim()
                .TrimStart('(')
                .TrimEnd(')')
                .ToUpper();
            
            _paramsList.Clear();
            
            var i = 0;
            var balance = 0;
            var currentParam = new StringBuilder();
            while (i <= p.Length)
            {
                if (i == p.Length || (p[i] == ',' && balance == 0))
                {
                    _paramsList.Add(currentParam.ToString());
                    currentParam.Clear();
                }
                else
                {
                    currentParam.Append(p[i]);
                    
                    if (p[i] == '(')
                        balance++;
                    if (p[i] == ')')
                        balance--;
                }

                i++;
            }
        }
    }

    public class FormulaConverter2
    {
        private const string ValFunctionName = "VAL";

        private List<string> _exceptionList = new List<string>
        {
            ValFunctionName
        };

        private readonly List<string> _operators = new List<string>
        {
            "+", "-", "*", "/", "==", "!=", ">", "<", ">=", "<=", "&&", "||"
        };

        private readonly List<char> _operatorChars = new List<char>
        {
            '+', '-', '*', '/', '=', '!', '>', '<', '&', '|', '(', ')'
        };

        public string PrepareFormula(string formulaIn, string contextName)
        {
            var sb = new StringBuilder(formulaIn.Replace("$c", contextName));
            sb = sb
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

            sb = Process(sb);

            return sb.ToString();
        }

        public void AddToExceptionList(string functionName)
        {
            if (_exceptionList.Any(x => x == functionName.ToUpper()))
                return;

            _exceptionList.Add(functionName.ToUpper());
        }

        #region private methods

        private int ProcessFunction(Statement item, int i, StringBuilder currentStatement, StringBuilder formula)
        {
            item.FunctionName = currentStatement.ToString().Trim().ToUpper();
            currentStatement.Clear();
            var balance = 0;
            while (i < formula.Length)
            {
                currentStatement.Append(formula[i]);
                
                if (formula[i] == '(')
                    balance++;
                if (formula[i] == ')')
                    balance--;

                if (formula[i] == ')' && balance == 0)
                    break;
                i++;
            }
            
            item.SetFunctionParameters(currentStatement);
            i++;

            return i;
        }
        
        public StringBuilder Process(StringBuilder formula)
        {
            // разбираем формулу
            var currentStatement = new StringBuilder();
            var statementTree = new List<Statement>();
            formula = formula.Replace(" ", "");

            var i = 0;
            var currentStart = 0;
            while (i <= formula.Length)
            {
                if (i == formula.Length || _operatorChars.Contains(formula[i]))
                {
                    var type = GetStatementType(currentStatement);
                    var item = new Statement
                    {
                        Type = type
                    };
                    
                    switch (type)
                    {
                        case ElementType.Function:
                            i = ProcessFunction(item, i, currentStatement, formula);
                            break;
                        case ElementType.Address:
                        case ElementType.Numeric:
                            item.OriginExpression = formula
                                .ToString(currentStart, i - currentStart)
                                .Trim()
                                .ToUpper();
                            break;
                    }
                    
                    statementTree.Add(item);
                    var nextOperator = GetOperator(formula, i);
                    if (nextOperator != null)
                    {
                        statementTree.Add(nextOperator);
                        i += nextOperator.OriginExpression.Length;
                        currentStart = i;
                        currentStatement.Clear();
                    }
                    else
                    {
                        i++;
                    }
                }
                else
                {
                    currentStatement.Append(formula[i]);
                    i++;
                }
            }
            
            // собираем формулу
            var outFormula = new StringBuilder();
            foreach (var statement in statementTree)
            {
                if (statement.Type == ElementType.Function)
                    outFormula.Append($"{statement.FunctionName}({statement.FunctionParams}) ");
                else
                    outFormula.Append($"{statement.ProcessedExpression} ");
            }

            outFormula = outFormula.Remove(outFormula.Length - 1, 1);
            return outFormula;
        }

        private Statement GetOperator(StringBuilder formula, int i)
        {
            if (formula.Length == i)
                return null;
            
            if (!_operatorChars.Contains(formula[i + 1]))
            {
                return new Statement
                {
                    Type = ElementType.Operator,
                    OriginExpression = formula[i].ToString()
                };
            }
            else
            {
                return new Statement
                {
                    Type = ElementType.Operator,
                    OriginExpression = $"{formula[i]}{formula[i + 1]}"
                };
            }
        }

        private ElementType GetStatementType(StringBuilder statement)
        {
            var st = statement.ToString().Trim();
            if (Regex.IsMatch(st, @"R-*\d*C-*\d*"))
                return ElementType.Address;

            if (decimal.TryParse(st, out _))
                return ElementType.Numeric;

            return ElementType.Function;
        }

        #endregion
    }
}