using System.Linq;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters;
using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Models;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class ExcelFormulaConverterTests
{
    [TestCase("=G1+(SUM(A1:C1)*2)", @"R1C7+(SUM(""R1C1:R1C3"")*2)")]
    [TestCase("A1:C1", @"R1C1:R1C3")]
    [TestCase("$A$1:C1", @"R1C1:R1C3")]
    [TestCase("=SUM(A1:C1)", @"SUM(""R1C1:R1C3"")")]
    [TestCase("=SIN(100)", @"SIN(100)", 1)]
    [TestCase("=IF(1>2,0,1)", @"IIF(1>2,0,1)")]
    public void ExcelFormulaConverterTests_Tests(string excelFormula, string expectedFormula, int expectedExceptionCount = 0)
    {
        var converter = new ExcelFormulaConverter();
        var result = converter.Convert(excelFormula);
        
        Assert.AreEqual(result.Formula, expectedFormula);
        Assert.AreEqual(result.ConvertExceptions.Count(), expectedExceptionCount);
    }

    [TestCase(@"R1C1:R1C3", "A1:C1")]
    [TestCase(@"R1C1:R1C3", "R1C1:R1C3", 0, CellAddressFormat.RowCellFormat)]
    [TestCase(@"SUM(""R1C1:R1C3"")", "=SUM(A1:C1)")]
    [TestCase(@"IIF(1>2,0,1)", "=IF(1>2,0,1)")]
    [TestCase(@"R1C7+(SUM(""R1C1:R1C3"")*2)", "=G1+(SUM(A1:C1)*2)")]
    public void SheetFormulaConverterTests_Tests(string sheetFormula,
        string expectedFormula,
        int expectedExceptionCount = 0,
        CellAddressFormat addressFormat = CellAddressFormat.DefaultExcelFormat)
    {
        var converter = new SheetFormulaConverter();
        var result = converter.Convert(sheetFormula);
        
        Assert.AreEqual(result.Formula, expectedFormula);
        Assert.AreEqual(result.ConvertExceptions.Count(), expectedExceptionCount);
    }
}