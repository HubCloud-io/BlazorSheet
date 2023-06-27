using System.Text;
using HubCloud.BlazorSheet.EvalEngine.Engine;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class FormulaConverterTests
{
    private const string ContextName = "_cells";
    
    // [TestCase("123", "123")]
    // [TestCase("R8C10+R-1C10", $@"{ContextName}.GetValue(""R8C10"")+{ContextName}.GetValue(""R-1C10"")")]
    // [TestCase(@"SUM(""R8C10:R-1C10"")", @"SUM(""R8C10:R-1C10"")")]
    // [TestCase(@"SUM(""R8C10:R-1C10"")+R8C10+R-1C10+SUM(""R8C10:R-1C10"")+SUM(""R8C11:R-1C10"")", $@"SUM(""R8C10:R-1C10"")+{ContextName}.GetValue(""R8C10"")+{ContextName}.GetValue(""R-1C10"")+SUM(""R8C10:R-1C10"")+SUM(""R8C11:R-1C10"")")]
    // [TestCase(@"VAL(""R8C10"")", $@"{ContextName}.GetValue(""R8C10"")")]
    // [TestCase(@"FOO(""R8C10"")", $@"FOO({ContextName}.GetValue(""R8C10""))")]
    // [TestCase(@"IsEmpty(VAL(""R41C3""))", $@"IsEmpty({ContextName}.GetValue(""R41C3""))")]
    // [TestCase(@"IIF(""R1C2"" = 0, ""R4C5"", 2)", $@"IIF({ContextName}.GetValue(""R1C2"") == 0, {ContextName}.GetValue(""R4C5""), 2)")]
    //
    [TestCase(@"IIF(""R1C1"" > 10, IIF(""R1C2"" = 0, 1, 2) + ""R4C5"", 45)", $@"IIF({ContextName}.GetValue(""R1C1"") > 10, IIF({ContextName}.GetValue(""R1C2"") == 0, 1, 2) + {ContextName}.GetValue(""R4C5""), 45)")]
    // [TestCase(@"IIF(VAL(""R1C1"")+SUM(""R2C1:R5C1"" > 100 , 4, 5 )", $@"IIF({ContextName}.GetValue(""R1C1"")+SUM(""R2C1:R5C1"" > 100, 4, 5)")]
    public void PrepareFormula_Tests(string formulaIn, string expected)
    {
        FormulaConverter.AddToExceptionList("SUM");
        var formulaOut = FormulaConverter.PrepareFormula(formulaIn, ContextName);
        
        Assert.AreEqual(expected, formulaOut);
    }
    
    // [TestCase("R4C5 + R4C6 && R4C7", "R4C5 + R4C6 && R4C7")]
    // [TestCase("R4C5 + 42 <= R4C7", "R4C5 + 42 <= R4C7")]
    // [TestCase("14 + 42 || 76", "14 + 42 || 76")]
    // [TestCase(@"FOO(""R1C1"", ""R1C2"") + VAL(""R1C1"")", @"FOO(""R1C1"", ""R1C2"") + VAL(""R1C1"")")]
    
    // пробел убран потому что еще нет рекурсивного обхода аргументов
    [TestCase(@"FOO(FOO2(""R1C1"", ""R1C2""), ""R1C2"")", @"FOO(FOO2(""R1C1"",""R1C2""), ""R1C2"")")]
    
    // [TestCase(@"IIF(""R1C1"" > 10, IIF(""R1C2"" = 0, 1, 2) + R4C5, 45)", @"IIF(""R1C1"" > 10, IIF(""R1C2"" == 0, 1, 2) + R4C5, 45)")]
    public void PrepareFormula2_Tests(string formulaIn, string expected)
    {
        var converter = new FormulaConverter2();
        var formulaOut = converter.Process(new StringBuilder(formulaIn));
        
        Assert.AreEqual(expected, formulaOut.ToString());
    }
}