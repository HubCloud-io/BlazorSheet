using System;
using HubCloud.BlazorSheet.Core.Models;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class SheetCellAddressTests
{
    [TestCase("R1C3","", 1, 3)]
    [TestCase("Sheet1!r3c5","Sheet1", 3, 5)]
    [TestCase("Sheet1 !R 2C 2","Sheet1", 2, 2)]
    [TestCase("Sheet1 !R3C4","Sheet1", 3, 4)]
    [TestCase("Sheet1!R1C1","Sheet1", 1, 1)]
    public void Parse_ValidCellAddress_SetsPropertiesCorrectly(string address, string sheetName, int row, int column)
    {
        var cellAddress = new SheetCellAddress(address);

        Assert.AreEqual(sheetName, cellAddress.SheetName);
        Assert.AreEqual(row, cellAddress.Row);
        Assert.AreEqual(column, cellAddress.Column);
    }
    
    [Test]
    public void Parse_InvalidCellAddress_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new SheetCellAddress("InvalidAddress"));
    }

    [Test]
    public void Parse_EmptyCellAddress_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new SheetCellAddress(string.Empty));
    }

    [Test]
    public void Parse_NullCellAddress_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new SheetCellAddress(null));
    }
}