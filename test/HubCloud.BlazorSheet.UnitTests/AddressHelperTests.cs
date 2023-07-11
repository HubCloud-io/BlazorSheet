using HubCloud.BlazorSheet.EvalEngine.Helpers;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class AddressHelperTests
{
    [TestCase("123", "123")]
    [TestCase("R1C3", "R1C3")]
    [TestCase("R5C6", "F5")]
    public void Tests(string inputAddress, string expected)
    {
        var outputAddress = AddressHelper.ConvertAddress(inputAddress);
        Assert.AreEqual(outputAddress, expected);
    }
}