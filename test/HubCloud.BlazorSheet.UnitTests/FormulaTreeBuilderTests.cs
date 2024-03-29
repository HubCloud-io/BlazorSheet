﻿using System.Text;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors.Helpers;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors.Models;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class FormulaTreeBuilderTests : FormulaTreeBuilder
{
    [TestCase("123", "123")]
    [TestCase("R4C5 + R4C6 && R4C7", "R4C5+R4C6&&R4C7")]
    [TestCase("R4C5 + 42 <= R4C7", "R4C5+42<=R4C7")]
    [TestCase("14 + 42 || 76", "14+42||76")]
    [TestCase(@"FOO(""R1C1"", ""R1C2"") + FOO3(""R1C1"")", @"FOO(""R1C1"",""R1C2"")+FOO3(""R1C1"")")]
    [TestCase(@"FOO(FOO2(""R1C1"", ""R1C2""), ""R1C2"")", @"FOO(FOO2(""R1C1"",""R1C2""),""R1C2"")")]
    [TestCase(@"SUM(""R4C5:R4C8"")", @"SUM(""R4C5:R4C8"")")]
    [TestCase(@"FOO(""R1C1"", ""R1C2"")", @"FOO(""R1C1"",""R1C2"")")]
    [TestCase(@"IIF(""R1C1"" > 10, 1, 2)", @"IIF(""R1C1"">10,1,2)")]
    [TestCase(@"IIF(""R1C1"" > 10, IIF(""R1C2"" = 0, 1, 2) + R4C5, 45)", @"IIF(""R1C1"">10,IIF(""R1C2""=0,1,2)+R4C5,45)")]
    [TestCase(@"FOO(FOO2(FOO3(""R1C1"") + FOO2(""R1C1"" + ""R1C1"")))", @"FOO(FOO2(FOO3(""R1C1"")+FOO2(""R1C1""+""R1C1"")))")]
    [TestCase(@"PascalCaseName(""R1C1"", ""R1C2"")", @"PascalCaseName(""R1C1"",""R1C2"")")]
    [TestCase("R8C10 + R-1C-1", "R8C10+R-1C-1")]
    [TestCase("R[-1]C10 - R[-1]C11", "R[-1]C10-R[-1]C11")]
    [TestCase("(R8C10 + R[-1]C[-1]) - R8C10", "(R8C10+R[-1]C[-1])-R8C10")]
    [TestCase(@"VAL(""R8C10"").ToUpper()", @"VAL(""R8C10"").ToUpper()")]
    [TestCase(@"VAL(""R8C10"").IndexOf(""w"")", @"VAL(""R8C10"").IndexOf(""w"")")]
    [TestCase(@"VAL(""R34C3"").Day", @"VAL(""R34C3"").Day")]
    [TestCase(@"VAL(""R57C3"") && VAL(""R57C4"")", @"VAL(""R57C3"")&&VAL(""R57C4"")")]
    [TestCase(@"(VAL(""R55C3"") < 5) + (VAL(""R55C4"") > 10)", @"(VAL(""R55C3"")<5)+(VAL(""R55C4"")>10)")]
    public void FormulaTreeBuilder_Tests(string formulaIn, string expected)
    {
        var builder = new FormulaTreeBuilder();
        builder.AddToExceptionList("SUM");
        
        var formulaTree = builder.BuildStatementTree(formulaIn);
        var formulaOut = builder.BuildFormula(formulaTree);
        
        Assert.AreEqual(expected, formulaOut?.ToString());
    }

    [TestCase(@"FOO", "(", ElementType.Function)]
    [TestCase(@"R57C3", null, ElementType.Address)]
    [TestCase(@"B4", null, ElementType.ExcelAddress)]
    [TestCase(@"AO54", null, ElementType.ExcelAddress)]
    [TestCase(@"A4:B6", null, ElementType.ExcelAddressRange)]
    [TestCase(@"$B$4", null, ElementType.ExcelAddress)]
    public void GetStatementType_Tests(string statement, string nextSymbol, ElementType expectedType)
    {
        var statementType = GetStatementType(new StringBuilder(statement), nextSymbol);
        
        Assert.AreEqual(statementType, expectedType);
    }
}