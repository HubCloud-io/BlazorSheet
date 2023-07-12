using System.Collections.Generic;
using System.Linq;
using System.Text;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverter.Abstractions;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverter.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors.Models;
using HubCloud.BlazorSheet.EvalEngine.Helpers;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters
{
    public class ExcelFormulaConverter : IExcelToSheetConverter
    {
        private readonly Dictionary<string, string> _functionDict = new Dictionary<string, string>
        {
            { "SUM", "SUM" }
        };
        
        public ConvertResult Convert(string excelFormula)
        {
            var treeBuilder = new FormulaTreeBuilder();
            var statementTree = treeBuilder.BuildStatementTree(excelFormula.Trim().TrimStart('='));

            statementTree = ProcessTree(statementTree, out var exceptionList);

            var convertResult = new ConvertResult();
            convertResult.Formula = treeBuilder
                .BuildFormula(statementTree)
                .ToString();
            convertResult.ConvertExceptions = exceptionList;

            return convertResult;
        }

        private List<Statement> ProcessTree(List<Statement> statementTree, out List<ConvertException> exceptionList)
        {
            exceptionList = new List<ConvertException>();
            foreach (var statement in statementTree)
            {
                switch (statement.Type)
                {
                    case ElementType.Function:
                        statement.ProcessedStatement = ProcessFunction(statement, out var functionExceptionList);
                        if (functionExceptionList?.Any() == true)
                            exceptionList.AddRange(functionExceptionList);
                        break;
                    case ElementType.ExcelAddress:
                        statement.ProcessedStatement = AddressHelper.ConvertAddress(statement.OriginStatement);
                        break;
                    case ElementType.ExcelAddressRange:
                        statement.ProcessedStatement = ProcessExcelAddressRange(statement.OriginStatement, out var convertException);
                        if (convertException != null)
                            exceptionList.Add(convertException);
                        break;
                    default:
                        statement.ProcessedStatement = statement.OriginStatement;
                        break;
                }
            }

            return statementTree;
        }

        private string ProcessExcelAddressRange(string excelAddressRange, out ConvertException exception)
        {
            exception = null;
            var arr = excelAddressRange.Split(':').ToArray();
            if (arr.Length != 2)
            {
                exception = new ConvertException
                {
                    Statement = excelAddressRange,
                    ExceptionDescription = "Excel address parse fail"
                };
                return excelAddressRange;
            }

            return $"{AddressHelper.ConvertAddress(arr.First())}:{AddressHelper.ConvertAddress(arr.Last())}";
        }
        
        private string ProcessFunction(Statement statement, out List<ConvertException> exceptionList)
        {
            exceptionList = new List<ConvertException>();
            var processedStatement = new StringBuilder();
            
            if (_functionDict.TryGetValue(statement.FunctionName.ToUpper(), out var convertedName))
                processedStatement.Append($"{convertedName}(");
            else
                processedStatement.Append($"{statement.FunctionName}(");
            
            var argCnt = 1;
            foreach (var p in statement.FunctionParams)
            {
                foreach (var innerStatement in p.InnerStatements)
                {
                    var st = ProcessTree(new List<Statement> {innerStatement}, out var innerExceptionList);
                    if (innerExceptionList?.Any() == true)
                        exceptionList.AddRange(innerExceptionList);

                    processedStatement.Append($"{st.FirstOrDefault()?.ProcessedStatement}");
                }

                if (argCnt++ < statement.FunctionParams.Count)
                    processedStatement.Append(",");
            }

            processedStatement.Append(')');
            return processedStatement.ToString();
        }
    }
}