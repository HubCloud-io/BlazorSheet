using HubCloud.BlazorSheet.Core.Models;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class UniversalValueTests
{
    [TestCase(1, 2, 3)]
    public void Plus_DecimalValues_SumValue(decimal value1, decimal value2, decimal check)
    {
        var uValue1 = new UniversalValue(value1);
        var uValue2 = new UniversalValue(value2);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(check, result.Value);

    }
}