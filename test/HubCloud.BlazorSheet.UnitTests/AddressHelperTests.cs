using HubCloud.BlazorSheet.EvalEngine.Helpers;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class AddressHelperTests
{
    [TestCase("123", "123")]
    [TestCase("Foobar", "Foobar")]
    [TestCase("R1C3", "R1C3")]
    [TestCase("F5", "R5C6")]
    [TestCase("AO3", "R3C41")]
    public void Tests(string inputAddress, string expected)
    {
        var outputAddress = AddressHelper.ConvertAddress(inputAddress);
        Assert.AreEqual(outputAddress, expected);
    }
}