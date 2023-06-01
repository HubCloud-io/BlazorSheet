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

    [Test]
    public void IndexerGet_ValidIndices_ReturnsCorrectValue()
    {
        _sheetData[5, 5] = "TestValue";
        Assert.AreEqual("TestValue", _sheetData[5, 5]);
    }

    [Test]
    public void IndexerSet_ValidIndices_SetsCorrectValue()
    {
        _sheetData[5, 5] = "TestValue";
        Assert.AreEqual("TestValue", _sheetData[5, 5]);
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