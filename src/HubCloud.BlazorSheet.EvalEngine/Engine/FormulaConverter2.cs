using System.Collections.Generic;
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
        Address,
        ValFunction,
        ExceptionFunction
    }

    public class Statement
    {
        public ElementType Type { get; set; }
        public string OriginStatement { get; set; }
        public string ProcessedStatement { get; set; }
        public string FunctionName { get; set; }
        public List<string> FunctionParamsList { get; set; }
        public List<Statement> InnerStatements { get; set; }
    }

    public class FormulaConverter2
    {
        private const string ValFunctionName = "VAL";

        private List<string> _exceptionList = new List<string>
        {
            ValFunctionName
        };

        // private readonly List<string> _operators = new List<string>
        // {
        //     "+", "-", "*", "/", "==", "!=", ">", "<", ">=", "<=", "&&", "||"
        // };

        private readonly List<char> _operatorChars = new List<char>
        {
            '+', '-', '*', '/', '=', '!', '>', '<', '&', '|', '(', ')'
        };

        public string PrepareFormula(string formulaIn, string contextName)
        {
            var sb = new StringBuilder(formulaIn)
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

            // formula to tree
            var statementTree = GetStatementTree(sb);

            // process formula tree
            statementTree = ProcessTree(statementTree, contextName);

            // from processed tree to formula
            sb = BuildFormula(statementTree);

            return sb.ToString();
        }

        public void AddToExceptionList(string functionName)
        {
            if (_exceptionList.Any(x => x == functionName.ToUpper()))
                return;

            _exceptionList.Add(functionName.ToUpper());
        }


        #region private methods

        private List<Statement> ProcessTree(List<Statement> statementTree, string contextName)
        {
            foreach (var statement in statementTree)
            {
                switch (statement.Type)
                {
                    case ElementType.Function:
                        statement.ProcessedStatement = $"{statement.FunctionName}(";
                        var argCnt = 1;
                        foreach (var p in statement.InnerStatements)
                        {
                            var st = ProcessTree(new List<Statement> {p}, contextName);
                            var processed = st.FirstOrDefault()?.ProcessedStatement;
                            statement.ProcessedStatement += $"{processed}";
                            if (argCnt++ < statement.InnerStatements.Count)
                                statement.ProcessedStatement += ",";
                        }
                        statement.ProcessedStatement += ")";
                        break;
                    case ElementType.ValFunction:
                        statement.ProcessedStatement = ProcessValFunction(statement, contextName);
                        break;
                    case ElementType.Address:
                        statement.ProcessedStatement = ProcessAddress(statement.OriginStatement, contextName);
                        break;
                    case ElementType.ExceptionFunction:
                        statement.ProcessedStatement = statement.OriginStatement;
                        break;
                    case ElementType.Numeric:
                    case ElementType.Operator:
                        statement.ProcessedStatement = statement.OriginStatement;
                        break;
                }
            }

            return statementTree;
        }

        private string ProcessAddress(string arg, string contextName)
            => $@"{contextName}.GetValue(""{arg.TrimStart('"').TrimEnd('"')}"")";

        private string ProcessValFunction(Statement valStatement, string contextName)
        {
            // ToDo: exception
            var arg = valStatement.FunctionParamsList.First();
            var processedStatement = ProcessAddress(arg, contextName);

            return processedStatement;
        }

        private StringBuilder BuildFormula(List<Statement> statementTree)
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
                    outFormula.Append($"{statement.ProcessedStatement}");
                }
            }

            return outFormula;
        }

        private List<Statement> GetStatementTree(StringBuilder formula)
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

            item.FunctionParamsList = GetParameters(currentStatement);
            if (item.Type != ElementType.ExceptionFunction)
            {
                var innerStatements = new List<Statement>();
                foreach (var p in item.FunctionParamsList)
                {
                    var sb = new StringBuilder(p);
                    var statementTree = GetStatementTree(sb);
                    innerStatements.AddRange(statementTree);
                }

                if (innerStatements.Any())
                    item.InnerStatements = innerStatements;
            }

            item.OriginStatement = $"{item.FunctionName}{currentStatement}";

            i++;

            return i;
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

        private List<string> GetParameters(StringBuilder parameters)
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

            var paramsList = new List<string>();

            var i = 0;
            var balance = 0;
            var currentParam = new StringBuilder();
            while (i <= p.Length)
            {
                if (i == p.Length || (p[i] == ',' && balance == 0))
                {
                    paramsList.Add(currentParam.ToString());
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