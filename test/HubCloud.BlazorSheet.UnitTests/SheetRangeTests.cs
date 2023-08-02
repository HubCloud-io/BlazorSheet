using System;
using HubCloud.BlazorSheet.Core.Models;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class SheetRangeTests
{
    
    [Test]
    public void Test_SheetRange_Constructor()
    {
        var sheetRange = new SheetRange(1, 2, 3, 4);
        Assert.AreEqual(1, sheetRange.RowStart);
        Assert.AreEqual(2, sheetRange.ColumnStart);
        Assert.AreEqual(3, sheetRange.RowEnd);
        Assert.AreEqual(4, sheetRange.ColumnEnd);
        Assert.AreEqual(string.Empty, sheetRange.SheetName);
    }

    [TestCase("Sheet1!R1C1:R2C2", 2, 2, 1, 1, 2, 2)]
    [TestCase("Sheet1!R1C1:Sheet2!R2C2", 2, 2, 1, 1, 2, 2)]
    public void Test_SheetRange_String_Constructor(string address,
        int currentRow,
        int currentCol,
        int rowStart,
        int colStart,
        int rowEnd,
        int colEnd)
    {
        var sheetRange = new SheetRange(address, currentRow, currentCol);
        Assert.AreEqual(rowStart, sheetRange.RowStart);
        Assert.AreEqual(colStart, sheetRange.ColumnStart);
        Assert.AreEqual(rowEnd, sheetRange.RowEnd);
        Assert.AreEqual(colEnd, sheetRange.ColumnEnd);
        Assert.AreEqual("Sheet1", sheetRange.SheetName);
    }

    [Test]
    public void SheetRange_OneCellRange_Constructor()
    {
        var sheetRange = new SheetRange("R2C3", 1, 1);
        
        Assert.AreEqual(2, sheetRange.RowStart);
        Assert.AreEqual(2, sheetRange.RowEnd);
        Assert.AreEqual(3, sheetRange.ColumnStart);
        Assert.AreEqual(3, sheetRange.ColumnEnd);
    }
    
    [TestCase(1, 2, 3, 4, 2, 3, true)]
    [TestCase(1, 2, 3, 4, 1, 2, true)]
    [TestCase(1, 2, 3, 4, 5, 6, false)]
    [TestCase(1, 2, 3, 4, 1, 1, false)]
    public void Test_IsCellInRange(int rowStart, int colStart, int rowEnd, int colEnd, int testRow, int testCol, bool expectedInRange)
    {
        var sheetRange = new SheetRange(rowStart, colStart, rowEnd, colEnd);
        Assert.AreEqual(expectedInRange, sheetRange.IsCellInRange(testRow, testCol));
    }
}