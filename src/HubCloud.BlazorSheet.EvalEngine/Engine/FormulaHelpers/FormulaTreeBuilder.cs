using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.FormulaHelpers
{
    public class FormulaTreeBuilder
    {
        private const string ValFunctionName = "VAL";

        private readonly List<char> _operatorChars = new List<char>
        {
            '+', '-', '*', '/', '=', '!', '>', '<', '&', '|', '(', ')'
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
            index = i+1;
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
                    var type = GetStatementType(currentStatement);
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
                        case ElementType.NumericOrOther:
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
                    outFormula.Append($"{statement.FunctionName}");
                    outFormula.Append("(");
                    var argIndex = 1;
                    foreach (var param in statement.FunctionParams)
                    {
                        var currentArg = BuildFormula(param.InnerStatements);
                        outFormula.Append(currentArg);
                        if (argIndex < statement.FunctionParams.Count)
                        {
                            outFormula.Append(",");
                            argIndex++;
                        }
                    }

                    outFormula.Append(")");
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
            if (_exceptionList.Any(x => x.Trim().ToUpper() == functionName.Trim().ToUpper()))
                return;

            _exceptionList.Add(functionName);
        }

        #region private methods

        private ElementType GetStatementType(StringBuilder statement)
        {
            var st = statement.ToString().Trim();
            if (st == ValFunctionName || st.Contains($"{ValFunctionName}("))
                return ElementType.ValFunction;

            if (st != ValFunctionName && _exceptionList.Contains(st))
                return ElementType.ExceptionFunction;

            if (Regex.IsMatch(st, @"R-*\d*C-*\d*:R-*\d*C-*\d*"))
                return ElementType.AddressRange;

            if (Regex.IsMatch(st, @"R-*\d*C-*\d*"))
                return ElementType.Address;

            if (string.IsNullOrEmpty(st) || decimal.TryParse(st, out _))
                return ElementType.NumericOrOther;
            
            if (st.First() == '"' && st.Last() == '"')
                return ElementType.NumericOrOther;

            return ElementType.Function;
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
                    OriginStatement = formula[i].ToString()
                };
            }
            else
            {
                return new Statement
                {
                    Type = ElementType.Operator,
                    OriginStatement = $"{formula[i]}{formula[i + 1]}"
                };
            }
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

        #endregion
    }
}