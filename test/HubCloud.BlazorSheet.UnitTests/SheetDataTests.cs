using HubCloud.BlazorSheet.Core.Models;
using NUnit.Framework;
using System;

namespace HubCloud.BlazorSheet.UnitTests;


[TestFixture]
public class SheetDataTests
{
    private SheetData _sheetData;

    [SetUp]
    public void Setup()
    {
        _sheetData = new SheetData(10, 10);
    }

    [TestCase(1,1)]
    [TestCase(5,5)]
    [TestCase(1,10)]
    [TestCase(10,1)]
    [TestCase(10,10)]
    public void IndexerGet_ValidIndices_ReturnsCorrectValue(int r, int c)
    {
        _sheetData[r, c] = "TestValue";
        Assert.AreEqual("TestValue", _sheetData[r, c]);
    }
    
    [Test]
    public void IndexerGet_RowIndexOutOfRange_ThrowsIndexOutOfRangeException()
    {
        Assert.Throws<IndexOutOfRangeException>(() =>
        {
            var test = _sheetData[11, 5];
        });
    }

    [Test]
    public void IndexerSet_RowIndexOutOfRange_ThrowsIndexOutOfRangeException()
    {
        Assert.Throws<IndexOutOfRangeException>(() => { _sheetData[11, 5] = "TestValue"; });
    }

    [Test]
    public void IndexerGet_ColumnIndexOutOfRange_ThrowsIndexOutOfRangeException()
    {
        Assert.Throws<IndexOutOfRangeException>(() =>
        {
            var test = _sheetData[5, 11];
        });
    }

    [Test]
    public void IndexerSet_ColumnIndexOutOfRange_ThrowsIndexOutOfRangeException()
    {
        Assert.Throws<IndexOutOfRangeException>(() => { _sheetData[5, 11] = "TestValue"; });
    }
}