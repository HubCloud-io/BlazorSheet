using HubCloud.BlazorSheet.Core.Models;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using ExpressoFunctions.FunctionLibrary;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class UniversalValueArithmeticsOperatorsTests
{
    
    [Test]
    public void Plus_UvDecimalAndUvDecimal_SumValue()
    {
        var uValue1 = new UniversalValue(1M);
        var uValue2 = new UniversalValue(2M);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(3M, result.Value);
    }
    
    [Test]
    public void Plus_UvDecimalAndUvDouble_SumValue()
    {
        var uValue1 = new UniversalValue(1.5M);
        var uValue2 = new UniversalValue(2.5);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(4M, result.Value);
    }
    
    [Test]
    public void Plus_UvDecimalValueAndDecimal_SumValue()
    {
        var uValue1 = new UniversalValue(1M);
        
        var result = uValue1 + 2M;
        
        Assert.AreEqual(3M, result.Value);
    }
    
    [Test]
    public void Plus_UvDecimalAndInt_SumValue()
    {
        var uValue1 = new UniversalValue(1M);
        
        var result = uValue1 + 2;
        
        Assert.AreEqual(3M, result.Value);
    }
    
    [Test]
    public void Plus_UvIntAndInt_SumValue()
    {
        var uValue1 = new UniversalValue(1);
        
        var result = uValue1 + 2;
        
        Assert.AreEqual(3M, result.Value);
    }
    
    [Test]
    public void Plus_UvDecimalAndNull_SumValue()
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
    public void Multiply_UvDecimalAndUvDecimal_Result()
    {
        var value1 = new UniversalValue(2m);
        var value2 = new UniversalValue(3m);

        var result = value1 * value2;
        
        Assert.AreEqual(6m, result.Value);
    }
    
    [Test]
    public void Multiply_DecimalAndUvDecimal_Result()
    {
        var value1 = 2.5m;
        var value2 = new UniversalValue(2m);

        var result = value1 * value2;
        
        Assert.AreEqual(5m, result.Value);
    }
    
    [Test]
    public void Multiply_IntAndUvDecimal_Result()
    {
        var value1 = 2;
        var value2 = new UniversalValue(3m);

        var result = value1 * value2;
        
        Assert.AreEqual(6m, result.Value);
    }
}