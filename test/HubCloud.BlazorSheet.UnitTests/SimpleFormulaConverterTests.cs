using System.Collections.Generic;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class SimpleFormulaConverterTests
{
    private const string ContextName = "_cells";

    [TestCase(@"VAL(""R8C10"")", $@"VAL(""R8C10"")")]
    [TestCase("R8C10", $@"{ContextName}.GetValue(""R8C10"")")]
    [TestCase(@"SUM(""R8C10:R-1C10"")", @"SUM(""R8C10:R-1C10"")")]
    [TestCase(@"SUM(""R8C10:R1C10"") + R8C10 + R1C10 + SUM(""R8C10:R1C10"") + SUM(""R8C11:R1C10"")", $@"SUM(""R8C10:R1C10"")+{ContextName}.GetValue(""R8C10"")+{ContextName}.GetValue(""R1C10"")+SUM(""R8C10:R1C10"")+SUM(""R8C11:R1C10"")")]
    [TestCase(@"FOO(""R8C10"")", $@"FOO({ContextName}.GetValue(""R8C10""))")]
    [TestCase(@"IsEmpty(VAL(""R41C3""))", $@"IsEmpty(VAL(""R41C3""))")]
    [TestCase(@"IsTrue(""R8C10"" = 10)", $@"IsTrue({ContextName}.GetValue(""R8C10"")==10)")]
    [TestCase(@"IIF(""R1C2"" = 0, ""R4C5"", 2)", $@"IIF({ContextName}.GetValue(""R1C2"")==0,{ContextName}.GetValue(""R4C5""),2)")]
    [TestCase(@"IIF((VAL(""R1C1"") + SUM(""R2C1:R5C1"")) > 100, 4, 5)", $@"IIF((VAL(""R1C1"")+SUM(""R2C1:R5C1""))>100,4,5)")]
    [TestCase(@"IIF(""R1C1"" > 10, IIF(""R1C2"" = 0, 1, 2) + ""R4C5"", 45)", $@"IIF({ContextName}.GetValue(""R1C1"")>10,IIF({ContextName}.GetValue(""R1C2"")==0,1,2)+{ContextName}.GetValue(""R4C5""),45)")]
    [TestCase(@"VAL(""RC"") + VAL(""RC2"") + VAL(""R4C"")", $@"VAL(""RC"")+VAL(""RC2"")+VAL(""R4C"")")]
    [TestCase("R8C10 + R-1C10", $@"{ContextName}.GetValue(""R8C10"")+{ContextName}.GetValue(""R-1C10"")")]
    [TestCase("R-1C10 - R-1C11", $@"{ContextName}.GetValue(""R-1C10"")-{ContextName}.GetValue(""R-1C11"")")]
    [TestCase("(R8C10 + R-1C-1) - R8C10", $@"({ContextName}.GetValue(""R8C10"")+{ContextName}.GetValue(""R-1C-1""))-{ContextName}.GetValue(""R8C10"")")]
    [TestCase(@"VAL(""R8C10"").ToUpper()", @$"VAL(""R8C10"").ToUpper()")]
    [TestCase(@"Val(""R2C3"").BeginYear()", @$"Val(""R2C3"").BeginYear()")]
    [TestCase(@"Sum(""R5C3:R5C5"")", @$"Sum(""R5C3:R5C5"")")]
    public void FormulaProcessor_Tests(string formulaIn, string expected)
    {
        var converter = new TreeFormulaProcessor();
        
        var exceptionList = new List<string> { "SUM" };
        var formulaOut = converter.PrepareFormula(formulaIn, ContextName, exceptionList);
        
        Assert.AreEqual(expected, formulaOut);
    }
}