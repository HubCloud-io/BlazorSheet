using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.DependencyAnalyzer;
using Newtonsoft.Json;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class SheetDependencyAnalyzerTests
{
    [Test]
    public void GetOrderCells2_Test()
    {
        // Arrange
        var sheetSettings = new SheetSettings
        {
            RowsCount = 5,
            ColumnsCount = 5
        };
        
        var sheet = new Sheet(sheetSettings)
        {
            Name = "main"
        };

        sheet.GetCell(1, 1).Value = 100;
        
        var cell12 = sheet.GetCell(1, 2);
        cell12.Formula = @"VAL(""R1C1"") + 10";
        
        var cell13 = sheet.GetCell(1, 3);
        cell13.Formula = @"VAL(""R1C4"") + 30";
        
        var cell14 = sheet.GetCell(1, 4);
        cell14.Formula = @"VAL(""R1C2"") + 20";
        
        var cell15 = sheet.GetCell(1, 5);
        cell15.Formula = @"VAL(""R1C1"") + 50";
       
        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        var analyzer = new SheetDependencyAnalyzer(workbook.FirstSheet);
        var orderedCells = analyzer.OrderCellsForCalc2();
        
        // Assert
        Assert.AreEqual(orderedCells[0].Uid, cell12.Uid);
        Assert.AreEqual(orderedCells[1].Uid, cell14.Uid);
        Assert.AreEqual(orderedCells[2].Uid, cell15.Uid);
        Assert.AreEqual(orderedCells[3].Uid, cell13.Uid);
    }

    [Test]
    public void GetOrderCells2_CircularDependency_Test()
    {
        // Arrange
        var sheetSettings = new SheetSettings
        {
            RowsCount = 5,
            ColumnsCount = 5
        };
        
        var sheet = new Sheet(sheetSettings)
        {
            Name = "main"
        };

        sheet.GetCell(1, 1).Value = 100;
        
        var cell12 = sheet.GetCell(1, 2);
        cell12.Formula = @"VAL(""R1C1"") + VAL(""R1C2"")";
       
        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        var analyzer = new SheetDependencyAnalyzer(workbook.FirstSheet);
        try
        {
            var orderedCells = analyzer.OrderCellsForCalc2();
        }
        catch (Exception e)
        {
            // Assert
            Assert.That(e is EvaluateException);
            Assert.That(e.Message.Trim() == "Formula circular dependency problem in cells: R1C2;");
            return;
        }
        
        Assert.Fail();
    }

    // [Test]
    // public void GetOrderCells2_JsonExample_Test()
    // {
    //     // Arrange
    //     var str = System.IO.File.ReadAllText(@"d:\test\sheetSettings.json");
    //     var sheetSettings = JsonConvert.DeserializeObject<SheetSettings>(str);
    //     var workbook = new Workbook();
    //     workbook.AddSheet(new Sheet(sheetSettings));
    //     
    //     var str2 = System.IO.File.ReadAllText(@"d:\test\sheetSettings2.json");
    //     var sheetSettings2 = JsonConvert.DeserializeObject<SheetSettings>(str2);
    //     var workbook2 = new Workbook();
    //     workbook2.AddSheet(new Sheet(sheetSettings2));
    //
    //     // var badCells = new[]
    //     // {
    //     //     new ValueAddress("R56C16"),
    //     //     new ValueAddress("R62C16"),
    //     //     new ValueAddress("R64C4"),
    //     //     new ValueAddress("R65C4"),
    //     //     new ValueAddress("R66C4"),
    //     //     new ValueAddress("R67C4"),
    //     //     new ValueAddress("R68C4"),
    //     //     new ValueAddress("R118C16"),
    //     //     new ValueAddress("R124C16")
    //     // };
    //     //
    //     // foreach (var badCell in badCells)
    //     // {
    //     //     var cell = workbook2.FirstSheet.Cells.FirstOrDefault(x => workbook2.FirstSheet.CellAddressSlim(x).Equals(badCell));
    //     //     if (cell != null)
    //     //     {
    //     //         cell.Formula = null;
    //     //         cell.Value = 0;
    //     //     }
    //     // }
    //     
    //     // Act
    //     var analyzer = new SheetDependencyAnalyzer(workbook2.FirstSheet);
    //     try
    //     {
    //         var orderedCells = analyzer.OrderCellsForCalc2();
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         Assert.Fail();
    //     }
    //     
    //     Assert.Pass();
    // }
    
    [Test]
    public void DependencyCells_Order_Test()
    {
        var workbook = BuildTestWorkbook();
        
        var analyzer = new SheetDependencyAnalyzer(workbook.FirstSheet);
        var dependencyCells = analyzer.GetDependencyCells(new SheetCellAddress(2, 2))
            .ToArray();
        
        Assert.AreEqual(dependencyCells.Count(), 4);
        Assert.AreEqual(dependencyCells[0].Formula, @"VAL(""R2C2"") + 40");
        Assert.AreEqual(dependencyCells[1].Formula, @"SUM(""R2C2:R2C2"")");
        Assert.AreEqual(dependencyCells[2].Formula, @"VAL(""R5C2"")+VAL(""R2C2"")");
        Assert.AreEqual(dependencyCells[3].Formula, @"VAL(""R2C2"")+VAL(""R4C2"")");
    }

    [Test]
    public void DependencyCells_Test()
    {
        // Arrange
        var sheetSettings = new SheetSettings
        {
            RowsCount = 10,
            ColumnsCount = 10
        };

        var sheet = new Sheet(sheetSettings)
        {
            Name = "main"
        };

        sheet.GetCell(1, 4).Formula = @"SUM(""RC1:R[2]C3"")";

        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        var analyzer = new SheetDependencyAnalyzer(workbook.FirstSheet);
        var dependencyCells = analyzer.GetDependencyCells(new SheetCellAddress(2, 2))
            .ToArray();
        
        // Assert
        Assert.AreEqual(dependencyCells.Count(), 1);
        Assert.AreEqual(dependencyCells[0].Formula, @"SUM(""RC1:R[2]C3"")");
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
        
        var normalizedAddress1 = SheetDependencyAnalyzer.NormalizeAddress("R[-1]C1", cellAddress);
        var normalizedAddress2 = SheetDependencyAnalyzer.NormalizeAddress("R1C[-1]", cellAddress);
        var normalizedAddress3 = SheetDependencyAnalyzer.NormalizeAddress("R[-1]C[-1]", cellAddress);
        
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
        var state1 = SheetDependencyAnalyzer.IsAddressInRange("R2C2", "R1C1:R3C3", new SheetCellAddress(1, 1));
        var state2 = SheetDependencyAnalyzer.IsAddressInRange("R2C2", "R3C3:R5C5", new SheetCellAddress(1, 1));
        var state3 = SheetDependencyAnalyzer.IsAddressInRange("R[1]C[1]", "R1C1:R3C3", new SheetCellAddress(1, 1));
        var state4 = SheetDependencyAnalyzer.IsAddressInRange("RC2", "R1C1:R3C3", new SheetCellAddress(2, 1));
        var state5 = SheetDependencyAnalyzer.IsAddressInRange("R2C", "R1C1:R3C3", new SheetCellAddress(1, 2));
        
        Assert.IsTrue(state1);
        Assert.IsFalse(state2);
        Assert.IsTrue(state3);
        Assert.IsTrue(state4);
        Assert.IsTrue(state5);
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