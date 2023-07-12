using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class ExcelFormulaConverterTests
{
    [TestCase("=G1+(SUM(A1:C1)*2)", @"R1C7+(SUM(""R1C1:R1C3"")*2)")]
    [TestCase("A1:C1", @"R1C1:R1C3")]
    [TestCase("$A$1:C1", @"R1C1:R1C3")]
    [TestCase("SUM(A1:C1)", @"SUM(""R1C1:R1C3"")")]
    public void ExcelFormulaConverterTests_Tests(string excelFormula, string expected)
    {
        var converter = new ExcelFormulaConverter();
        var result = converter.Convert(excelFormula);
        
        Assert.AreEqual(result.Formula, expected);
    }
}