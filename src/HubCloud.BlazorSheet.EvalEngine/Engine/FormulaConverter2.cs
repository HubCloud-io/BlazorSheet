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
        Address
    }

    public class Statement
    {
        public ElementType Type { get; set; }
        public string Expression { get; set; }
        public string FunctionName { get; set; }
        public string FunctionParams { get; set; }
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

            sb = Foo(sb);

            return sb.ToString();
        }

        public void AddToExceptionList(string functionName)
        {
            if (_exceptionList.Any(x => x == functionName.ToUpper()))
                return;

            _exceptionList.Add(functionName.ToUpper());
        }

        #region private methods

        private StringBuilder Foo(StringBuilder formula)
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
                    switch (type)
                    {
                        case ElementType.Function:
                            break;
                        case ElementType.Address:
                            var item = new Statement();
                            item.Type = type;
                            item.Expression = formula.ToString(currentStart, i - currentStart).Trim().ToUpper();
                            statementTree.Add(item);
                            
                            var op = GetOperator(formula, i);
                            if (op != null)
                            {
                                statementTree.Add(op);
                                i += op.Expression.Length;
                                currentStart = i;
                                currentStatement.Clear();
                            }
                            else
                            {
                                i++;
                            }

                            break;
                        case ElementType.Numeric:
                            break;
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
                outFormula.Append($"{statement.Expression} ");
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
                    Expression = formula[i].ToString()
                };
            }
            else
            {
                return new Statement
                {
                    Type = ElementType.Operator,
                    Expression = $"{formula[i]}{formula[i + 1]}"
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