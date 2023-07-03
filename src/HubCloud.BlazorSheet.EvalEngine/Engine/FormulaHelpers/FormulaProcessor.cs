using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.FormulaHelpers
{
    public class FormulaProcessor
    {
        // private readonly List<string> _operators = new List<string>
        // {
        //     "+", "-", "*", "/", "==", "!=", ">", "<", ">=", "<=", "&&", "||"
        // };

        private FormulaTreeBuilder _treeBuilder;
        
        public string PrepareFormula(string formulaIn, string contextName, List<string> exceptionFunctionsList = null)
        {
            _treeBuilder = new FormulaTreeBuilder(exceptionFunctionsList);
            
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

            var statementTree = _treeBuilder.BuildStatementTree(sb);

            // process formula tree
            statementTree = ProcessTree(statementTree, contextName);

            sb = _treeBuilder.BuildFormula(statementTree);

            return sb.ToString();
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
                        foreach (var p in statement.FunctionParams)
                        {
                            foreach (var innerStatement in p.InnerStatements)
                            {
                                var st = ProcessTree(new List<Statement> {innerStatement}, contextName);
                                statement.ProcessedStatement += $"{st.FirstOrDefault()?.ProcessedStatement}";
                            }
                            
                            if (argCnt++ < statement.FunctionParams.Count)
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
                    case ElementType.NumericOrOther:
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
            var arg = valStatement.FunctionParams.First();
            var processedStatement = ProcessAddress(arg.Origin, contextName);

            return processedStatement;
        }

        #endregion
    }
}