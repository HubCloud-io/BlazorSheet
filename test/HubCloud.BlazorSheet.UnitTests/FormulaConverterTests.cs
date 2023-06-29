using System.Collections.Generic;
using System.Text;
using DynamicExpresso;
using HubCloud.BlazorSheet.EvalEngine.Engine;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaHelpers;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class FormulaConverterTests
{
    private const string ContextName = "_cells";
    
    // passed testcases
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
    public void FormulaTreeBuilder_Tests(string formulaIn, string expected)
    {
        var builder = new FormulaTreeBuilder();
        builder.AddToExceptionList("SUM");
        
        var formulaTree = builder.BuildStatementTree(new StringBuilder(formulaIn));
        var formulaOut = builder.BuildFormula(formulaTree);
        
        Assert.AreEqual(expected, formulaOut?.ToString());
    }
    
    // passed testcases
    [TestCase(@"VAL(""R8C10"")", $@"{ContextName}.GetValue(""R8C10"")")]
    [TestCase("R8C10", $@"{ContextName}.GetValue(""R8C10"")")]
    [TestCase(@"SUM(""R8C10:R-1C10"")", @"SUM(""R8C10:R-1C10"")")]
    [TestCase(@"SUM(""R8C10:R1C10"") + R8C10 + R1C10 + SUM(""R8C10:R1C10"") + SUM(""R8C11:R1C10"")", $@"SUM(""R8C10:R1C10"")+{ContextName}.GetValue(""R8C10"")+{ContextName}.GetValue(""R1C10"")+SUM(""R8C10:R1C10"")+SUM(""R8C11:R1C10"")")]
    [TestCase(@"FOO(""R8C10"")", $@"FOO({ContextName}.GetValue(""R8C10""))")]
    [TestCase(@"IsEmpty(VAL(""R41C3""))", $@"ISEMPTY({ContextName}.GetValue(""R41C3""))")]
    
    // ToDo: проблемное выражение 'R-1C10'
    // [TestCase("R8C10 + R-1C10", $@"{ContextName}.GetValue(""R8C10"")+{ContextName}.GetValue(""R-1C10"")")]
    // [TestCase(@"IsTrue(""R8C10"" = 10)", $@"IsTrue({ContextName}.GetValue(""R8C10"")==10)")]
    // [TestCase(@"IIF(""R1C2"" = 0, ""R4C5"", 2)", $@"IIF({ContextName}.GetValue(""R1C2"")==0,{ContextName}.GetValue(""R4C5""),2)")]
    // [TestCase(@"IIF(""R1C1"" > 10, IIF(""R1C2"" = 0, 1, 2) + ""R4C5"", 45)", $@"IIF({ContextName}.GetValue(""R1C1"")>10,IIF({ContextName}.GetValue(""R1C2"")==0,1,2)+{ContextName}.GetValue(""R4C5""),45)")]
    // [TestCase(@"IIF((VAL(""R1C1"") + SUM(""R2C1:R5C1"")) > 100, 4, 5)", $@"IIF(({ContextName}.GetValue(""R1C1"")+SUM(""R2C1:R5C1""))>100,4,5)")]
    public void PrepareFormula2_Tests(string formulaIn, string expected)
    {
        var converter = new FormulaConverter2();
        
        var exceptionList = new List<string> { "SUM" };
        var formulaOut = converter.PrepareFormula(formulaIn, ContextName, exceptionList);
        
        Assert.AreEqual(expected, formulaOut);
    }
}