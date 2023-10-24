using System.Collections.Generic;
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
    [TestCase("=SUM(A1)", @"SUM(""R1C1"")")]
    [TestCase("=SUM(A1,C1,A1:C1)", @"SUM(""R1C1"",""R1C3"",""R1C1:R1C3"")")]
    [TestCase("=SUM(R1C1:R1C3)", @"SUM(""R1C1:R1C3"")")]
    [TestCase("=SUM(R[1]C[1]:R1C3)", @"SUM(""R[1]C[1]:R1C3"")")]
    public void ExcelFormulaConverter_Tests(string excelFormula,
        string expectedFormula,
        int expectedExceptionCount = 0)
    {
        var converter = new ExcelFormulaConverter();
        var result = converter.Convert(excelFormula);
        
        Assert.AreEqual(result.Formula, expectedFormula);
        Assert.AreEqual(result.ConvertExceptions.Count(), expectedExceptionCount);
    }

    [TestCase(@"R1C3", "=C1")]
    [TestCase(@"R1C1:R1C3", "=A1:C1")]
    [TestCase(@"R1C1:R1C3", "=R1C1:R1C3", 0, CellAddressFormat.R1C1Format)]
    [TestCase(@"SUM(""R1C1:R1C3"")", "=SUM(A1:C1)")]
    [TestCase(@"IIF(1>2,0,1)", "=IF(1>2,0,1)")]
    [TestCase(@"R1C7+(SUM(""R1C1:R1C3"")*2)", "=G1+(SUM(A1:C1)*2)")]
    [TestCase(@"SUM(""R1C1"",""R1C2"")", @"=SUM(R1C1,R1C2)", 0, CellAddressFormat.R1C1Format)]
    [TestCase(@"SUM(""R2C1"",""R1C2"")", @"=SUM(A2,B1)", 0, CellAddressFormat.A1Format)]
    public void SheetFormulaConverter_Tests(string sheetFormula,
        string expectedFormula,
        int expectedExceptionCount = 0,
        CellAddressFormat addressFormat = CellAddressFormat.A1Format)
    {
        var converter = new SheetFormulaConverter();
        var result = converter.Convert(sheetFormula, addressFormat);
        
        Assert.AreEqual(result.Formula, expectedFormula);
        Assert.AreEqual(result.ConvertExceptions.Count(), expectedExceptionCount);
    }
    
    [TestCase(@"SUM(""RC1"",""RC2"")", "=SUM(R4C1,R4C2)", 4, 1)]
    [TestCase(@"SUM(""R[0]C1"",""R[0]C2"")", "=SUM(R4C1,R4C2)", 4, 1)]
    [TestCase(@"SUM(""RC[5]"",""RC[6]"")", "=SUM(R2C6,R2C7)", 2, 1)]
    [TestCase(@"SUM(""R1C[-5]"",""R1C[-4]"")", "=SUM(R1C5,R1C6)", 4, 10)]
    [TestCase(@"SUM(""RC[-5]"",""RC[-4]"")", "=SUM(R2C5,R2C6)", 2, 10)]
    [TestCase(@"SUM(""R[1]C1:R1C3"")", "=SUM(R2C1:R1C3)", 1, 3)]
    public void SheetFormulaConverter_R1C1Format_Tests(string sheetFormula,
        string expectedFormula,
        int currentRow,
        int currentCol)
    {
        var converter = new SheetFormulaConverter();
        var result = converter.Convert(sheetFormula,
            CellAddressFormat.R1C1Format,
            currentRow,
            currentCol);
        
        Assert.AreEqual(result.Formula, expectedFormula);
    }

    [TestCase(@"SUM(""R[1]C1"",""R1C[1]"")", @"=SUM(A2,B1)", 1, 1)]
    public void SheetFormulaConverter_A1Format_Tests(string sheetFormula,
        string expectedFormula,
        int currentRow,
        int currentCol)
    {
        var converter = new SheetFormulaConverter();
        var result = converter.Convert(sheetFormula,
            CellAddressFormat.A1Format,
            currentRow,
            currentCol);
        
        Assert.AreEqual(result.Formula, expectedFormula);
    }
}