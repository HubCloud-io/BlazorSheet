using System.Collections.Generic;
using System.Linq;
using System.Text;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverter.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Abstractions;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors.Models;
using HubCloud.BlazorSheet.EvalEngine.Helpers;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters
{
    public class SheetFormulaConverter : ISheetToExcelConverter
    {
        public ConvertResult Convert(string excelFormula, CellAddressFormat cellAddressFormat = CellAddressFormat.DefaultExcelFormat)
        {
            var treeBuilder = new FormulaTreeBuilder();
            var statementTree = treeBuilder.BuildStatementTree(excelFormula.Trim());
            
            statementTree = ProcessTree(statementTree, cellAddressFormat, out var exceptionList);
            
            var convertResult = new ConvertResult();
            var formula = treeBuilder
                .BuildFormula(statementTree)
                .ToString();
            convertResult.Formula = $"={formula}";
            convertResult.ConvertExceptions = exceptionList;
            
            return convertResult;
        }

        #region private methods
        private List<Statement> ProcessTree(List<Statement> statementTree,
            CellAddressFormat cellAddressFormat,
            out List<ConvertException> exceptionList)
        {
            exceptionList = new List<ConvertException>();
            foreach (var statement in statementTree)
            {
                switch (statement.Type)
                {
                    case ElementType.Function:
                        statement.ProcessedStatement = ProcessFunction(statement, cellAddressFormat, out var functionExceptionList);
                        if (functionExceptionList?.Any() == true)
                            exceptionList.AddRange(functionExceptionList);
                        break;
                    case ElementType.Address when cellAddressFormat == CellAddressFormat.DefaultExcelFormat:
                        statement.ProcessedStatement = AddressHelper.ConvertRowCellToExcelAddress(statement.OriginStatement);
                        break;
                    case ElementType.AddressRange when cellAddressFormat == CellAddressFormat.DefaultExcelFormat:
                        statement.ProcessedStatement = ProcessAddressRange(statement.OriginStatement, out var convertException);
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
        
        private string ProcessAddressRange(string addressRange, out ConvertException exception)
        {
            exception = null;
            var arr = addressRange.Split(':');
            if (arr.Length != 2)
            {
                exception = new ConvertException
                {
                    Statement = addressRange,
                    ExceptionDescription = "Cell address parse fail"
                };
                return addressRange;
            }

            return $"{AddressHelper.ConvertRowCellToExcelAddress(arr.First())}:{AddressHelper.ConvertRowCellToExcelAddress(arr.Last())}";
        }

        private string ProcessFunction(Statement statement,
            CellAddressFormat cellAddressFormat,
            out List<ConvertException> exceptionList)
        {
            exceptionList = new List<ConvertException>();
            var processedStatement = new StringBuilder();
            
            if (FunctionMatchingHelper.SheetToExcelDict.TryGetValue(statement.FunctionName.ToUpper(), out var convertedName))
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
                var isAddressRange = p.InnerStatements.Count == 2 &&
                                     p.InnerStatements.Any(x => x.Type == ElementType.Address);
                
                foreach (var innerStatement in p.InnerStatements)
                {
                    var st = ProcessTree(new List<Statement> {innerStatement}, cellAddressFormat, out var innerExceptionList);
                    if (innerExceptionList?.Any() == true)
                        exceptionList.AddRange(innerExceptionList);

                    processedStatement.Append($"{st.FirstOrDefault()?.ProcessedStatement}");

                    if (isAddressRange)
                    {
                        isAddressRange = false;
                        processedStatement.Append(":");
                    }
                }

                if (argCnt++ < statement.FunctionParams.Count)
                    processedStatement.Append(",");
            }
            processedStatement.Append(')');

            return processedStatement.ToString();
        }

        #endregion
    }
}