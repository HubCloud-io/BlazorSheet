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
}