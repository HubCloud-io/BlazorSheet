using System.Linq;
using System.Text.RegularExpressions;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.DependencyAnalyzer;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class SheetDependencyAnalyzerTests
{
    [Test]
    public void NormalizeAddress_Simple_Test()
    {
        var cellAddress = new SheetCellAddress
        {
            Row = 1,
            Column = 1
        };

        var normalizedAddress = SheetDependencyAnalyzer.NormalizeAddress("R1C1", cellAddress);
        
        Assert.AreEqual(normalizedAddress, "R1C1");
    }
    
    [Test]
    public void NormalizeAddress_MinusValue_Test()
    {
        var cellAddress = new SheetCellAddress
        {
            Row = 2,
            Column = 2
        };
        
        var normalizedAddress1 = SheetDependencyAnalyzer.NormalizeAddress("R-1C1", cellAddress);
        var normalizedAddress2 = SheetDependencyAnalyzer.NormalizeAddress("R1C-1", cellAddress);
        var normalizedAddress3 = SheetDependencyAnalyzer.NormalizeAddress("R-1C-1", cellAddress);
        
        Assert.AreEqual(normalizedAddress1, "R1C1");
        Assert.AreEqual(normalizedAddress2, "R1C1");
        Assert.AreEqual(normalizedAddress3, "R1C1");
    }

    [Test]
    public void NormalizeAddress_NonValue_Test()
    {
        var cellAddress = new SheetCellAddress
        {
            Row = 2,
            Column = 2
        };
        
        var normalizedAddress1 = SheetDependencyAnalyzer.NormalizeAddress("RC1", cellAddress);
        var normalizedAddress2 = SheetDependencyAnalyzer.NormalizeAddress("R1C", cellAddress);
        
        Assert.AreEqual(normalizedAddress1, "R2C1");
        Assert.AreEqual(normalizedAddress2, "R1C2");
    }

    [Test]
    public void IsAddressInRange_Test()
    {
        var state1 = SheetDependencyAnalyzer.IsAddressInRange("R2C2", "R1C1:R3C3");
        var state2 = SheetDependencyAnalyzer.IsAddressInRange("R2C2", "R3C3:R5C5");
        
        Assert.IsTrue(state1);
        Assert.IsFalse(state2);
    }

    [Test]
    public void GetAddressListByRange_Test()
    {
        var list = SheetDependencyAnalyzer.GetAddressListByRange("R1C1:R2C3");
        
        Assert.AreEqual(list.Count, 6);
        Assert.AreEqual(list[0], "R1C1");
        Assert.AreEqual(list[1], "R1C2");
        Assert.AreEqual(list[2], "R1C3");
        Assert.AreEqual(list[3], "R2C1");
        Assert.AreEqual(list[4], "R2C2");
        Assert.AreEqual(list[5], "R2C3");
    }
}