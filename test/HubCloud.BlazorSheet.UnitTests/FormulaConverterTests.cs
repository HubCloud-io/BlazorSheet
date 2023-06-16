using HubCloud.BlazorSheet.Core.EvalEngine;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class FormulaConverterTests
{
    private const string ContextName = "_cells";
    
    [TestCase("123", "123")]
    [TestCase("R8C10+R-1C10", $@"{ContextName}.GetValue(""R8C10"")+{ContextName}.GetValue(""R-1C10"")")]
    [TestCase(@"SUM(""R8C10:R-1C10"")", @"SUM(""R8C10:R-1C10"")")]
    [TestCase(@"SUM(""R8C10:R-1C10"")+R8C10+R-1C10+SUM(""R8C10:R-1C10"")+SUM(""R8C11:R-1C10"")", $@"SUM(""R8C10:R-1C10"")+{ContextName}.GetValue(""R8C10"")+{ContextName}.GetValue(""R-1C10"")+SUM(""R8C10:R-1C10"")+SUM(""R8C11:R-1C10"")")]
    
    [TestCase("IIF(R1C2 = 0, R4C5, 2)", $@"IIF(R1C2 = 0, {ContextName}.GetValue(""R4C5""), 2)")]
    //[TestCase("IIF(R1C1 > 10, IIF(R1C2 = 0, 1, 2) + R4C5, 45)", $@"")]
    public void PrepareFormula_Tests(string formulaIn, string expected)
    {
        IFormulaConverter formulaConverter = new FormulaConverter2();
        var formulaOut = formulaConverter.PrepareFormula(formulaIn, ContextName);
        
        Assert.AreEqual(expected, formulaOut);
    }
}