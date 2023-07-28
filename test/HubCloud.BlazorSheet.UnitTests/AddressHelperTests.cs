using HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverters.Models;
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
    public void ExcelToRowCellAddress_Tests(string inputAddress, string expected)
    {
        var outputAddress = AddressHelper.ConvertExcelToRowCellAddress(inputAddress);
        Assert.AreEqual(outputAddress, expected);
    }

    [TestCase("123", "123")]
    [TestCase("Foobar", "Foobar")]
    [TestCase("R5C6", "F5")]
    [TestCase("R3C41", "AO3")]
    public void RowCellToExcelAddress_Tests(string inputAddress, string expected)
    {
        var outputAddress = AddressHelper.ConvertR1C1ToA1Address(inputAddress);
        Assert.AreEqual(outputAddress, expected);
    }
}