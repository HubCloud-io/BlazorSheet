﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverter.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Abstractions;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Helpers;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors.Helpers;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors.Models;
using HubCloud.BlazorSheet.EvalEngine.Helpers;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters
{
    /// <summary>
    /// Convert formula from BlazorSheet to Excel format
    /// </summary>
    public class SheetFormulaConverter : ISheetToExcelConverter
    {
        public ConvertResult Convert(string excelFormula,
            CellAddressFormat cellAddressFormat = CellAddressFormat.A1Format,
            int currentRow = 0,
            int currentCol = 0)
        {
            var treeBuilder = new FormulaTreeBuilder();
            var statementTree = treeBuilder.BuildStatementTree(excelFormula.Trim());
            
            statementTree = ProcessTree(statementTree,
                cellAddressFormat,
                out var exceptionList,
                currentRow,
                currentCol);
            
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
            CellAddressFormat excelAddressFormat,
            out List<ConvertException> exceptionList,
            int currentRow = 0,
            int currentCol = 0)
        {
            exceptionList = new List<ConvertException>();
            foreach (var statement in statementTree)
            {
                switch (statement.Type)
                {
                    case ElementType.Function:
                        statement.ProcessedStatement = ProcessFunction(statement, excelAddressFormat, out var functionExceptionList, currentRow, currentCol);
                        if (functionExceptionList?.Any() == true)
                            exceptionList.AddRange(functionExceptionList);
                        break;
                    case ElementType.Address when excelAddressFormat == CellAddressFormat.A1Format:
                        statement.ProcessedStatement = AddressHelper.ConvertR1C1ToA1Address(statement.OriginStatement, currentRow, currentCol);
                        break;
                    case ElementType.Address when excelAddressFormat == CellAddressFormat.R1C1Format:
                        statement.ProcessedStatement = AddressHelper.ProcessR1C1Address(statement.OriginStatement, currentRow, currentCol);
                        break;
                    case ElementType.AddressRange when excelAddressFormat == CellAddressFormat.A1Format:
                        statement.ProcessedStatement = ProcessAddressRange(statement.OriginStatement, out var convertException, currentRow, currentCol);
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
        
        private string ProcessAddressRange(string addressRange, out ConvertException exception, int currentRow, int currentCol)
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

            var rangeStart = AddressHelper.ConvertR1C1ToA1Address(arr.First(), currentRow, currentCol);
            var rangeEnd = AddressHelper.ConvertR1C1ToA1Address(arr.Last(), currentRow, currentCol);

            return $"{rangeStart}:{rangeEnd}";
        }

        private string ProcessFunction(Statement statement,
            CellAddressFormat cellAddressFormat,
            out List<ConvertException> exceptionList,
            int currentRow,
            int currentCol)
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
                    var st = ProcessTree(new List<Statement> {innerStatement}, cellAddressFormat, out var innerExceptionList, currentRow, currentCol);
                    if (innerExceptionList?.Any() == true)
                        exceptionList.AddRange(innerExceptionList);

                    var arg = st.FirstOrDefault();
                    if (arg?.Type == ElementType.Address)
                        arg.ProcessedStatement = arg.ProcessedStatement.Trim('"');
                    
                    processedStatement.Append($"{arg?.ProcessedStatement}");

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