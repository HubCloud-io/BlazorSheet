using HubCloud.BlazorSheet.Core.Models;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class UniversalValueTests
{
   
    [Test]
    public void Plus_DecimalAndDecimal_SumValue()
    {
        var uValue1 = new UniversalValue(1M);
        var uValue2 = new UniversalValue(2M);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(3M, result.Value);
    }
    
    [Test]
    public void Plus_DecimalAndNull_SumValue()
    {
        var uValue1 = new UniversalValue(1M);
        var uValue2 = new UniversalValue(null);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(1M, result.Value);
    }
    
    [Test]
    public void Plus_NullAndDecimal_SumValue()
    {
        var uValue1 = new UniversalValue(null);
        var uValue2 = new UniversalValue(2M);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(2M, result.Value);
    }
    
    [Test]
    public void Plus_DecimalAndInt_SumValue()
    {
        var uValue1 = new UniversalValue(1M);
        var uValue2 = new UniversalValue(1);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(2M, result.Value);
    }
    
    [Test]
    public void Plus_IntAndDecimal_SumValue()
    {
        var uValue1 = new UniversalValue(1);
        var uValue2 = new UniversalValue(1M);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(2M, result.Value);
    }

    [Test]
    public void Substring_StartIndex_SubstringValue()
    {
        var uv = new UniversalValue("qwerty");

        var result = uv.Substring(1);

        Assert.AreEqual("werty", result.Value);
    }

    [Test]
    public void Substring_StartIndexLength_SubstringValue()
    {
        var uv = new UniversalValue("qwerty");

        var result = uv.Substring(0, 1);

        Assert.AreEqual("q", result.Value);
    }

    [Test]
    public void ToUpper_ToUpperValue()
    {
        var uv = new UniversalValue("qwerty");

        var result = uv.ToUpper();

        Assert.AreEqual("QWERTY", result.Value);
    }

    [Test]
    public void ToLower_ToLowerValue()
    {
        var uv = new UniversalValue("QWERTY");

        var result = uv.ToLower();

        Assert.AreEqual("qwerty", result.Value);
    }

    [Test]
    public void IndexOf_StringValue_IndexOfValue()
    {
        var uv = new UniversalValue("QWERTY");

        var result = uv.IndexOf("W");

        Assert.AreEqual(1, result.Value);
    }

    [Test]
    public void Replace_OldValueNewValue_ReplaceValue()
    {
        var uv = new UniversalValue("QWERTY");

        var result = uv.Replace("WERT", "____");

        Assert.AreEqual("Q____Y", result.Value);
    }
}