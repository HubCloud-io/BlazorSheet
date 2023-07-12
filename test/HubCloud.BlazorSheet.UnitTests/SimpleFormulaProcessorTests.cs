using System.Collections.Generic;
using HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class SimpleFormulaProcessorTests
{
    private const string ContextName = "_cells";
    
    [TestCase("123", "123")]
    [TestCase("R4C5 + R4C6 && R4C7", @"VAL(""R4C5"") + VAL(""R4C6"") && VAL(""R4C7"")")]
    [TestCase("R4C5 + 42 <= R4C7", @"VAL(""R4C5"") + 42 <= VAL(""R4C7"")")]
    [TestCase("14 + 42 || 76", "14 + 42 || 76")]
    [TestCase(@"FOO(""R1C1"", ""R1C2"") + FOO3(""R1C1"")", @"FOO(VAL(""R1C1""), VAL(""R1C2"")) + FOO3(VAL(""R1C1""))")]
    [TestCase(@"FOO(FOO2(""R1C1"", ""R1C2""), ""R1C2"")", @"FOO(FOO2(VAL(""R1C1""), VAL(""R1C2"")), VAL(""R1C2""))")]
    [TestCase(@"SUM(""R4C5:R4C8"")", @"SUM(""R4C5:R4C8"")")]
    [TestCase(@"FOO(""R1C1"", ""R1C2"")", @"FOO(VAL(""R1C1""), VAL(""R1C2""))")]
    [TestCase(@"IIF(""R1C1"" > 10, 1, 2) - VAL(""R4C5"") + SUM(""R4C5:R4C8"") + SUM(""R4C5:R4C8"")",
              @"IIF(VAL(""R1C1"") > 10, 1, 2) - VAL(""R4C5"") + SUM(""R4C5:R4C8"") + SUM(""R4C5:R4C8"")")]
    [TestCase(@"IIF(""R1C1"" > 10, IIF(""R1C2"" = 0, 1, 2) + R4C5, 45)",
              @"IIF(VAL(""R1C1"") > 10, IIF(VAL(""R1C2"") == 0, 1, 2) + VAL(""R4C5""), 45)")]
    [TestCase(@"FOO(FOO2(FOO3(""R1C1"") + FOO2(""R1C1"" + ""R1C1"")))",
              @"FOO(FOO2(FOO3(VAL(""R1C1"")) + FOO2(VAL(""R1C1"") + VAL(""R1C1""))))")]
    [TestCase(@"PascalCaseName(""R1C1"", ""R1C2"")", @"PascalCaseName(VAL(""R1C1""), VAL(""R1C2""))")]
    [TestCase("R-1C10 - R-1C11", @"VAL(""R-1C10"") - VAL(""R-1C11"")")]
    [TestCase("(R8C10 + R-1C-1) - R8C10",
              @"(VAL(""R8C10"") + VAL(""R-1C-1"")) - VAL(""R8C10"")")]
    [TestCase(@"VAL(""R8C10"").ToUpper()", @"VAL(""R8C10"").ToUpper()")]
    [TestCase(@"VAL(""R8C10"").IndexOf(""w"")", @"VAL(""R8C10"").IndexOf(""w"")")]
    [TestCase(@"VAL(""R34C3"").Day", @"VAL(""R34C3"").Day")]
    [TestCase(@"VAL(""R57C3"") && VAL(""R57C4"")", @"VAL(""R57C3"") && VAL(""R57C4"")")]
    [TestCase(@"(VAL(""R55C3"") < 5) + (VAL(""R55C4"") > 10)",
              @"(VAL(""R55C3"") < 5) + (VAL(""R55C4"") > 10)")]
    [TestCase(@"VAL(""R8C10"") + VAL(""R-1C10"") && VAL(""RC10"")", @"VAL(""R8C10"") + VAL(""R-1C10"") && VAL(""RC10"")")]
    public void Process_Tests(string formulaIn, string expected)
    {
        var exceptionList = new List<string> { "SUM" };
        
        var builder = new SimpleFormulaProcessor(exceptionList);
        var formulaOut = builder.PrepareFormula(formulaIn, ContextName);
        
        Assert.AreEqual(expected, formulaOut);
    }
}