using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors
{
    public class FormulaTreeBuilder
    {
        private const string ValFunctionName = "VAL";

        private readonly List<char> _operatorChars = new List<char>
        {
            '+', '-', '*', '/', '=', '!', '>', '<', '&', '|', '(', ')', '.'
        };

        private readonly List<string> _compoundOperators = new List<string>
        {
            "==", "!=", ">=", "<=", "&&", "||"
        };

        private List<string> _exceptionList = new List<string>
        {
            ValFunctionName
        };

        public FormulaTreeBuilder(List<string> exceptionFunctionsList = null)
        {
            if (exceptionFunctionsList is null)
                return;
            foreach (var funcName in exceptionFunctionsList)
            {
                if (!_exceptionList.Any(x => x.Trim().ToUpper() == funcName.Trim().ToUpper()))
                    _exceptionList.Add(funcName);
            }
        }

        private bool IsAddressWithMinus(int i, StringBuilder formula)
        {
            if (formula[i] != '-')
                return false;

            // backward from minus
            var index = i;
            var st = new StringBuilder();
            var isR = false;
            while (index != 0)
            {
                if ((formula[index] == 'C' || formula[index] == 'c') && index < i - 1)
                    return false;

                st.Append(formula[index]);
                if (formula[index] == 'R' || formula[index] == 'r')
                {
                    isR = true;
                    break;
                }

                index--;
            }

            if (!isR) return true;

            st = Reverse(st);

            // forward from minus
            index = i + 1;
            while (index != formula.Length)
            {
                if (_operatorChars.Contains(formula[index]))
                {
                    break;
                }

                st.Append(formula[index]);
                index++;
            }

            var type = GetStatementType(st);
            return type == ElementType.Address;
        }

        public StringBuilder Reverse(StringBuilder sb)
        {
            char t;
            var end = sb.Length - 1;
            var start = 0;

            while (end - start > 0)
            {
                t = sb[end];
                sb[end] = sb[start];
                sb[start] = t;
                start++;
                end--;
            }

            return sb;
        }


        public List<Statement> BuildStatementTree(string formula)
            => BuildStatementTree(new StringBuilder(formula));

        public List<Statement> BuildStatementTree(StringBuilder formula)
        {
            var currentStatement = new StringBuilder();
            var statementTree = new List<Statement>();
            formula = formula.Replace(" ", "");

            var i = 0;
            var currentStart = 0;
            while (i <= formula.Length)
            {
                if (i == formula.Length || (_operatorChars.Contains(formula[i]) && !IsAddressWithMinus(i, formula)))
                {
                    var nextSymbol = i != formula.Length ? formula[i].ToString() : null;
                    var type = GetStatementType(currentStatement, nextSymbol);
                    var item = new Statement
                    {
                        Type = type
                    };

                    switch (type)
                    {
                        case ElementType.Function:
                        case ElementType.ValFunction:
                        case ElementType.ExceptionFunction:
                            i = ProcessFunction(item, i, currentStatement, formula);
                            break;
                        case ElementType.Address:
                        case ElementType.AddressRange:
                        case ElementType.ExcelAddress:
                        case ElementType.ExcelAddressRange:
                        case ElementType.NumericOrOther:
                        case ElementType.Property:
                            item.OriginStatement = formula
                                .ToString(currentStart, i - currentStart)
                                .Trim();
                            break;
                    }

                    statementTree.Add(item);
                    var nextOperator = GetOperator(formula, i);
                    if (nextOperator != null)
                    {
                        statementTree.Add(nextOperator);
                        i += nextOperator.OriginStatement.Length;
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

            return statementTree;
        }
        
        public StringBuilder BuildFormula(List<Statement> statementTree)
        {
            var outFormula = new StringBuilder();
            foreach (var statement in statementTree)
            {
                if (statement.Type == ElementType.Function)
                {
                    if (string.IsNullOrEmpty(statement.ProcessedStatement))
                    {
                        outFormula.Append($"{statement.FunctionName}(");
                        var argIndex = 1;
                        foreach (var param in statement.FunctionParams)
                        {
                            var isAddressParam = IsAddressParams(param.InnerStatements);
                            if (isAddressParam)
                                outFormula.Append('"');

                            var currentArg = BuildFormula(param.InnerStatements);
                            outFormula.Append(currentArg);

                            if (isAddressParam)
                                outFormula.Append('"');

                            if (argIndex++ < statement.FunctionParams.Count)
                                outFormula.Append(',');
                        }

                        outFormula.Append(")");
                    }
                    else
                    {
                        outFormula.Append(statement.ProcessedStatement);
                    }
                }
                else
                {
                    var st = string.IsNullOrEmpty(statement.ProcessedStatement)
                        ? statement.OriginStatement
                        : statement.ProcessedStatement;
                    outFormula.Append($"{st}");
                }
            }

            return outFormula;
        }

        public void AddToExceptionList(string functionName)
        {
            if (_exceptionList.Any(x => x == functionName.Trim().ToUpper()))
                return;

            _exceptionList.Add(functionName.Trim().ToUpper());
        }

        #region private methods

        private Regex _addressRangeRegex = new Regex(@"R-*\d*C-*\d*:R-*\d*C-*\d*", RegexOptions.Compiled);
        private Regex _addressRegex = new Regex(@"R-*\d*C-*\d*", RegexOptions.Compiled);
        private Regex _excelAddressRegex = new Regex(@"\$?[A-Z]+\$?\d+", RegexOptions.Compiled);
        private Regex _excelAddressRangeRegex = new Regex(@"\$?[A-Z]+\$?\d+:\$?[A-Z]+\$?\d+", RegexOptions.Compiled);

        protected ElementType GetStatementType(StringBuilder statement, string nextSymbol = null)
        {
            var st = statement.ToString().Trim();
            if (st.ToUpper() == ValFunctionName || st.ToUpper().Contains($"{ValFunctionName}("))
                return ElementType.ValFunction;

            if (st.ToUpper() != ValFunctionName && _exceptionList.Contains(st.ToUpper()))
                return ElementType.ExceptionFunction;

            if (_addressRangeRegex.IsMatch(st))
                return ElementType.AddressRange;

            if (_addressRegex.IsMatch(st))
                return ElementType.Address;

            if (string.IsNullOrEmpty(st) || decimal.TryParse(st, out _))
                return ElementType.NumericOrOther;

            if (st.First() == '"' && st.Last() == '"')
                return ElementType.NumericOrOther;

            if (nextSymbol == "(")
                return ElementType.Function;

            if (_excelAddressRangeRegex.IsMatch(st))
                return ElementType.ExcelAddressRange;

            if (_excelAddressRegex.IsMatch(st))
                return ElementType.ExcelAddress;

            return ElementType.Property;
        }

        private Statement GetOperator(StringBuilder formula, int i)
        {
            if (formula.Length == i)
                return null;

            var currentSymbol = formula[i].ToString();
            var nextSymbol = i + 1 < formula.Length ? formula[i + 1].ToString() : null;

            if (nextSymbol != null && _operatorChars.Contains(nextSymbol.First()))
            {
                var compoundOperator = $"{currentSymbol}{nextSymbol}";
                if (_compoundOperators.Contains(compoundOperator))
                {
                    return new Statement
                    {
                        Type = ElementType.Operator,
                        OriginStatement = compoundOperator
                    };
                }
            }

            return new Statement
            {
                Type = ElementType.Operator,
                OriginStatement = currentSymbol
            };
        }

        private int ProcessFunction(Statement item, int i, StringBuilder currentArgStatement, StringBuilder formula)
        {
            item.FunctionName = currentArgStatement
                .ToString()
                .Trim();

            currentArgStatement.Clear();
            var balance = 0;
            while (i < formula.Length)
            {
                currentArgStatement.Append(formula[i]);

                if (formula[i] == '(')
                    balance++;
                if (formula[i] == ')')
                    balance--;

                if (formula[i] == ')' && balance == 0)
                    break;
                i++;
            }

            item.FunctionParams = GetParameters(currentArgStatement, item);
            foreach (var p in item.FunctionParams)
            {
                var sb = new StringBuilder(p.Origin);
                if (GetStatementType(sb) == ElementType.AddressRange)
                {
                    p.InnerStatements = ProcessAddressRange(p.Origin);
                }
                else
                {
                    var statementTree = BuildStatementTree(sb);
                    p.InnerStatements.AddRange(statementTree);
                }
            }

            item.OriginStatement = $"{item.FunctionName}{currentArgStatement}";

            i++;

            return i;
        }

        private List<Statement> ProcessAddressRange(string address)
        {
            var addressArr = address.Split(':');
            var st = new List<Statement>();
            st.Add(new Statement
            {
                Type = ElementType.Address,
                OriginStatement = addressArr[0]
            });
            st.Add(new Statement
            {
                Type = ElementType.Address,
                OriginStatement = addressArr[1]
            });

            return st;
        }

        private List<FunctionParam> GetParameters(StringBuilder parameters, Statement parentStatement)
        {
            var p = parameters.ToString()
                .Trim();

            if (p.First() == '(')
            {
                p = p.Substring(1, p.Length - 1);
                if (p.Last() == ')')
                    p = p.Substring(0, p.Length - 1);
            }

            var paramsList = new List<FunctionParam>();

            var i = 0;
            var balance = 0;
            var currentParam = new StringBuilder();
            while (i <= p.Length)
            {
                if (i == p.Length || (p[i] == ',' && balance == 0))
                {
                    paramsList.Add(new FunctionParam(parentStatement)
                    {
                        Origin = currentParam.ToString()
                    });
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

            return paramsList;
        }
        
        private bool IsAddressParams(List<Statement> innerStatements)
        {
            return innerStatements.All(x => x.Type == ElementType.Address ||
                                            x.Type == ElementType.AddressRange ||
                                            x.Type == ElementType.ExcelAddress ||
                                            x.Type == ElementType.ExcelAddressRange);
        }

        #endregion
    }
}