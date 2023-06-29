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
        
        public List<Statement> BuildStatementTree(StringBuilder formula)
        {
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
                        case ElementType.ValFunction:
                        case ElementType.ExceptionFunction:
                            i = ProcessFunction(item, i, currentStatement, formula);
                            break;
                        case ElementType.Address:
                        case ElementType.Numeric:
                            item.OriginStatement = formula
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
                    foreach (var innerStatement in statement.InnerStatements)
                    {
                        var currentArg = BuildFormula(new List<Statement> {innerStatement});
                        outFormula.Append(currentArg);
                        if (argIndex < statement.InnerStatements.Count)
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
            if (_exceptionList.Any(x => x == functionName.ToUpper()))
                return;

            _exceptionList.Add(functionName.ToUpper());
        }
        
        #region private methods
        private ElementType GetStatementType(StringBuilder statement)
        {
            var st = statement.ToString().Trim();
            if (st == ValFunctionName || st.Contains($"{ValFunctionName}("))
                return ElementType.ValFunction;

            if (st != ValFunctionName && _exceptionList.Contains(st))
                return ElementType.ExceptionFunction;

            if (Regex.IsMatch(st, @"R-*\d*C-*\d*"))
                return ElementType.Address;

            if (decimal.TryParse(st, out _))
                return ElementType.Numeric;

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
        
        private int ProcessFunction(Statement item, int i, StringBuilder currentStatement, StringBuilder formula)
        {
            item.FunctionName = currentStatement
                .ToString()
                .Trim()
                .ToUpper();

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

            item.FunctionParams = GetParameters(currentStatement);
            if (item.Type != ElementType.ExceptionFunction)
            {
                var innerStatements = new List<Statement>();
                foreach (var p in item.FunctionParams)
                {
                    var sb = new StringBuilder(p.Origin);
                    var statementTree = BuildStatementTree(sb);
                    innerStatements.AddRange(statementTree);
                }

                if (innerStatements.Any())
                    item.InnerStatements = innerStatements;
            }

            item.OriginStatement = $"{item.FunctionName}{currentStatement}";

            i++;

            return i;
        }
        
        private List<FunctionParam> GetParameters(StringBuilder parameters)
        {
            var p = parameters.ToString()
                .Trim()
                .ToUpper();

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
                    paramsList.Add(new FunctionParam
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