using System.Linq;
using System.Text.RegularExpressions;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.DependencyAnalyzer;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class SheetDependencyAnalyzerTests
{
    [Test]
    public void DependencyCells_Order_Test()
    {
        var workbook = BuildTestWorkbook();
        
        var analyzer = new SheetDependencyAnalyzer(workbook.FirstSheet);
        var dependencyCells = analyzer.GetDependencyCells(new SheetCellAddress(2, 2)).ToArray();
        
        Assert.AreEqual(dependencyCells.Count(), 4);
        Assert.AreEqual(dependencyCells[0].Formula, @"VAL(""R2C2"") + 40");
        Assert.AreEqual(dependencyCells[1].Formula, @"SUM(""R2C2:R2C2"")");
        Assert.AreEqual(dependencyCells[2].Formula, @"VAL(""R5C2"")+VAL(""R2C2"")");
        Assert.AreEqual(dependencyCells[3].Formula, @"VAL(""R2C2"")+VAL(""R4C2"")");
    }

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
        var cell = new SheetCellAddress(1, 1);
        var state1 = SheetDependencyAnalyzer.IsAddressInRange("R2C2", "R1C1:R3C3", cell);
        var state2 = SheetDependencyAnalyzer.IsAddressInRange("R2C2", "R3C3:R5C5", cell);
        
        Assert.IsTrue(state1);
        Assert.IsFalse(state2);
    }

    [Test]
    public void GetAddressListByRange_Test()
    {
        var cellAddress = new SheetCellAddress(1, 1);
        var list = SheetDependencyAnalyzer.GetAddressListByRange("R1C1:R2C3", cellAddress);
        
        Assert.AreEqual(list.Count, 6);
        Assert.AreEqual(list[0], "R1C1");
        Assert.AreEqual(list[1], "R1C2");
        Assert.AreEqual(list[2], "R1C3");
        Assert.AreEqual(list[3], "R2C1");
        Assert.AreEqual(list[4], "R2C2");
        Assert.AreEqual(list[5], "R2C3");
    }
    
    [Test]
    public void GetAddressListByRange_Relative_Test()
    {
        var cellAddress = new SheetCellAddress(1, 1);
        var list = SheetDependencyAnalyzer.GetAddressListByRange("R1C1:R[1]C3", cellAddress);
        
        Assert.AreEqual(list.Count, 6);
        Assert.AreEqual(list[0], "R1C1");
        Assert.AreEqual(list[1], "R1C2");
        Assert.AreEqual(list[2], "R1C3");
        Assert.AreEqual(list[3], "R2C1");
        Assert.AreEqual(list[4], "R2C2");
        Assert.AreEqual(list[5], "R2C3");
    }
    
    [Test]
    public void GetAddressListByRange_Relative_Test_2()
    {
        var cellAddress = new SheetCellAddress(1, 1);
        var list = SheetDependencyAnalyzer.GetAddressListByRange("R1C1:R2C[2]", cellAddress);
        
        Assert.AreEqual(list.Count, 6);
        Assert.AreEqual(list[0], "R1C1");
        Assert.AreEqual(list[1], "R1C2");
        Assert.AreEqual(list[2], "R1C3");
        Assert.AreEqual(list[3], "R2C1");
        Assert.AreEqual(list[4], "R2C2");
        Assert.AreEqual(list[5], "R2C3");
    }
    
    #region private methods
    private Workbook BuildTestWorkbook()
    {
        var sheetSettings = new SheetSettings
        {
            RowsCount = 10,
            ColumnsCount = 6
        };

        var numberInputSettings = new SheetCellEditSettings()
        {
            ControlKind = CellControlKinds.NumberInput,
            NumberDigits = 2
        };
        
        sheetSettings.EditSettings.Add(numberInputSettings);
        
        var sheet = new Sheet(sheetSettings)
        {
            Name = "main"
        };

        sheet.GetCell(2, 1).Text = "Type value:";
        sheet.GetCell(2, 2).EditSettingsUid = numberInputSettings.Uid;
        sheet.GetCell(2, 2).Value = 1;
        sheet.GetCell(2, 2).Text = "1";
        
        SetDependentFormulas_CalcOrder(sheet);
        SetIndependentFormulas(sheet);

        var workbook = new Workbook();
        workbook.AddSheet(sheet);

        return workbook;
    }
    
    private void SetDependentFormulas_CalcOrder(Sheet sheet)
    {
        sheet.GetCell(3, 1).Text = @"VAL(""R2C2"")+VAL(""R4C2""):";
        sheet.GetCell(3, 2).Formula = @"VAL(""R2C2"")+VAL(""R4C2"")";
        
        sheet.GetCell(4, 1).Text = @"VAL(""R5C2"")+VAL(""R2C2""):";
        sheet.GetCell(4, 2).Formula = @"VAL(""R5C2"")+VAL(""R2C2"")";
        
        sheet.GetCell(5, 1).Text = @"VAL(""R2C2"") + 40:";
        sheet.GetCell(5, 2).Formula = @"VAL(""R2C2"") + 40";
        
        sheet.GetCell(6, 1).Text = @"SUM(""R2C2:R2C2""):";
        sheet.GetCell(6, 2).Formula = @"SUM(""R2C2:R2C2"")";
    }

    private void SetIndependentFormulas(Sheet sheet)
    {
        sheet.GetCell(2, 4).Text = "Other value:";
        sheet.GetCell(2, 5).Text = "100";
        sheet.GetCell(2, 5).Value = 100m;
        
        sheet.GetCell(3, 4).Text = "VAL(\"R2C5\")+VAL(\"R2C5\"): ";
        sheet.GetCell(3, 5).Formula = "VAL(\"R2C5\")+VAL(\"R2C5\")";
        
        sheet.GetCell(4, 4).Text = "VAL(\"R2C5\") - 50: ";
        sheet.GetCell(4, 5).Formula = "VAL(\"R2C5\")-50";
    }
    #endregion
}