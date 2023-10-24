using System.Collections.Generic;
using System.Linq;
using System.Text;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverter.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Abstractions;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Helpers;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors.Helpers;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters
{
    /// <summary>
    /// Convert formula from Excel to BlazorSheet
    /// </summary>
    public class ExcelFormulaConverter : IExcelToSheetConverter
    {
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

        #region private methods

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
                        statement.ProcessedStatement =
                            EvalEngine.Helpers.AddressHelper.ConvertExcelToRowCellAddress(statement.OriginStatement);
                        break;
                    case ElementType.ExcelAddressRange:
                        statement.ProcessedStatement =
                            ProcessExcelAddressRange(statement.OriginStatement, out var convertException);
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
            var arr = excelAddressRange.Split(':');
            if (arr.Length != 2)
            {
                exception = new ConvertException
                {
                    Statement = excelAddressRange,
                    ExceptionDescription = "Excel address parse fail"
                };
                return excelAddressRange;
            }

            return
                $"{EvalEngine.Helpers.AddressHelper.ConvertExcelToRowCellAddress(arr.First())}:{EvalEngine.Helpers.AddressHelper.ConvertExcelToRowCellAddress(arr.Last())}";
        }

        private string ProcessFunction(Statement statement, out List<ConvertException> exceptionList)
        {
            exceptionList = new List<ConvertException>();
            var processedStatement = new StringBuilder();

            if (FunctionMatchingHelper.ExcelToSheetDict.TryGetValue(statement.FunctionName.ToUpper(),
                    out var convertedName))
                processedStatement.Append($"{convertedName}");
            else
            {
                processedStatement.Append($"{statement.FunctionName}");
                exceptionList.Add(new ConvertException
                {
                    Statement = statement.FunctionName,
                    ExceptionDescription = "Can't find analog function"
                });
            }

            processedStatement.Append('(');

            var argCnt = 1;
            foreach (var p in statement.FunctionParams)
            {
                var isAddressParam = IsAddressParams(p.InnerStatements);
                if (isAddressParam)
                    processedStatement.Append('"');

                for (var i = 0; i < p.InnerStatements.Count; i++)
                {
                    var innerStatement = p.InnerStatements[i];
                    var st = ProcessTree(new List<Statement> {innerStatement}, out var innerExceptionList);
                    if (innerExceptionList?.Any() == true)
                        exceptionList.AddRange(innerExceptionList);

                    processedStatement.Append($"{st.FirstOrDefault()?.ProcessedStatement}");

                    if (i == 0 && IsAddressRangeParam(p.InnerStatements))
                        processedStatement.Append(":");
                }

                if (isAddressParam)
                    processedStatement.Append('"');

                if (argCnt++ < statement.FunctionParams.Count)
                    processedStatement.Append(",");
            }

            processedStatement.Append(')');
            return processedStatement.ToString();
        }

        private bool IsAddressRangeParam(List<Statement> innerStatements)
            => innerStatements.Count == 2 &&
               innerStatements.All(x => x.Type == ElementType.Address ||
                                        x.Type == ElementType.ExcelAddress);

        private bool IsAddressParams(List<Statement> innerStatements)
            => innerStatements.All(x => x.Type == ElementType.Address ||
                                        x.Type == ElementType.AddressRange ||
                                        x.Type == ElementType.ExcelAddress ||
                                        x.Type == ElementType.ExcelAddressRange);

        #endregion
    }
}