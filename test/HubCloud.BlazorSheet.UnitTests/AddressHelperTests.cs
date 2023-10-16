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
        var outputAddress = AddressHelper.ConvertR1C1ToA1Address(inputAddress, 0 , 0);
        Assert.AreEqual(outputAddress, expected);
    }

    [TestCase("R1C", "R1C5", 4, 5)]
    [TestCase("RC1", "R4C1", 4, 5)]
    [TestCase("R[0]C1", "R4C1", 4, 5)]
    [TestCase("R1C[0]", "R1C5", 4, 5)]
    [TestCase("R[-1]C5", "R3C5", 4, 5)]
    [TestCase("R3C[-2]", "R3C3", 4, 5)]
    [TestCase("R[-1]C", "R3C5", 4, 5)]
    [TestCase("RC[-2]", "R4C3", 4, 5)]
    public void ProcessR1C1Address_Tests(string inputAddress, string expected, int currentRow, int currentCol)
    {
        var outputAddress = AddressHelper.ProcessR1C1Address(inputAddress, currentRow, currentCol);
        Assert.AreEqual(outputAddress, expected);
    }

    [TestCase]
    public void GetColumnLetter_Tests()
    {
        var c1 = AddressHelper.GetColumnLetter("1");
        var c2 = AddressHelper.GetColumnLetter("2");
        
        var c25 = AddressHelper.GetColumnLetter("25");
        var c26 = AddressHelper.GetColumnLetter("26");
        var c27 = AddressHelper.GetColumnLetter("27");
        var c28 = AddressHelper.GetColumnLetter("28");
        
        var c400 = AddressHelper.GetColumnLetter("400");

        Assert.AreEqual(c25, "Y");
        Assert.AreEqual(c26, "Z");
        Assert.AreEqual(c27, "AA");
        Assert.AreEqual(c28, "AB");
    }
}