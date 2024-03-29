﻿using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Abstractions;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class CellShiftFormulaHelperTests
{
    [Test]
    public void OnRowAdd_AbsoluteAddresses_Test()
    {
        // Arrange
        var sheet = GetSheet();

        // value cells
        sheet.GetCell(6, 1).Value = 100;
        sheet.GetCell(1, 3).Value = 42;
        // formula cells
        sheet.GetCell(1, 1).Formula = @"VAL(""R6C1"")";
        sheet.GetCell(6, 3).Formula = @"VAL(""R6C1"")";
        sheet.GetCell(1, 2).Formula = @"SUM(""R6C1:R6C1"")+VAL(""R6C1"")";
        sheet.GetCell(6, 5).Formula = @"VAL(""R1C3"")";
        sheet.GetCell(1, 5).Formula = @"SUM(""R1C6:R7C6"")";

        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        IFormulaShifter formulaShifter = new CellShiftFormulaHelper(sheet);
        var shiftLog = formulaShifter.OnRowAdd(5);

        // Assert
        Assert.AreEqual(sheet.GetCell(1, 1).Formula, @"VAL(""R7C1"")");
        Assert.AreEqual(sheet.GetCell(6, 3).Formula, @"VAL(""R7C1"")");
        Assert.AreEqual(sheet.GetCell(1, 2).Formula, @"SUM(""R7C1:R7C1"")+VAL(""R7C1"")");
        Assert.AreEqual(sheet.GetCell(1, 5).Formula, @"SUM(""R1C6:R8C6"")");
    }
    
    [Test]
    public void OnRowAdd_RelativeAddresses_Test()
    {
        // Arrange
        var sheet = GetSheet();

        // value cells
        sheet.GetCell(4, 1).Value = 100;
        sheet.GetCell(6, 3).Value = 42;
        // formula cells
        sheet.GetCell(6, 1).Formula = @"VAL(""R[-2]C1"")";
        sheet.GetCell(4, 3).Formula = @"VAL(""R[2]C3"")";
        sheet.GetCell(1, 1).Formula = @"SUM(""R1C5:R[5]C5"")";

        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        IFormulaShifter formulaShifter = new CellShiftFormulaHelper(sheet);
        var shiftLog = formulaShifter.OnRowAdd(5);

        // Assert
        Assert.AreEqual(sheet.GetCell(6, 1).Formula, @"VAL(""R[-3]C1"")");
        Assert.AreEqual(sheet.GetCell(4, 3).Formula, @"VAL(""R[3]C3"")");
        Assert.AreEqual(sheet.GetCell(1, 1).Formula, @"SUM(""R1C5:R[6]C5"")");
    }
    
    [Test]
    public void OnColumnAdd_AbsoluteAddresses_Test()
    {
        // Arrange
        var sheet = GetSheet();

        // value cells
        sheet.GetCell(1, 4).Value = 100;
        sheet.GetCell(3, 1).Value = 42;
        // formula cells
        sheet.GetCell(1, 1).Formula = @"VAL(""R1C4"")";
        sheet.GetCell(3, 4).Formula = @"VAL(""R3C1"")";
        sheet.GetCell(5, 1).Formula = @"SUM(""R6C1:R6C4"")";
        sheet.GetCell(6, 1).Formula = @"SUM(""R7C4:R7C6"")";
        sheet.GetCell(7, 1).Formula = @"SUM(""R7C1:R7C2"")";

        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        IFormulaShifter formulaShifter = new CellShiftFormulaHelper(sheet);
        var shiftLog = formulaShifter.OnColumnAdd(3);

        // Assert
        Assert.AreEqual(sheet.GetCell(1, 1).Formula, @"VAL(""R1C5"")");
        Assert.AreEqual(sheet.GetCell(3, 4).Formula, @"VAL(""R3C1"")");
        Assert.AreEqual(sheet.GetCell(5, 1).Formula, @"SUM(""R6C1:R6C5"")");
        Assert.AreEqual(sheet.GetCell(6, 1).Formula, @"SUM(""R7C5:R7C7"")");
        Assert.AreEqual(sheet.GetCell(7, 1).Formula, @"SUM(""R7C1:R7C2"")");
    }

    [Test]
    public void OnColumnAdd_RelativeAddresses_Test()
    {
        // Arrange
        var sheet = GetSheet();

        // value cells
        sheet.GetCell(1, 4).Value = 100;
        sheet.GetCell(3, 1).Value = 42;
        
        // formula cells
        sheet.GetCell(1, 1).Formula = @"VAL(""R1C[3]"")";
        sheet.GetCell(3, 4).Formula = @"VAL(""R3C[-3]"")";
        sheet.GetCell(5, 1).Formula = @"SUM(""R6C1:R6C[4]"")";
        sheet.GetCell(7, 1).Formula = @"SUM(""R8C[5]:R8C[7]"")";
        
        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        IFormulaShifter formulaShifter = new CellShiftFormulaHelper(sheet);
        var shiftLog = formulaShifter.OnColumnAdd(3);

        // Assert
        Assert.AreEqual(sheet.GetCell(1, 1).Formula, @"VAL(""R1C[4]"")");
        Assert.AreEqual(sheet.GetCell(3, 4).Formula, @"VAL(""R3C[-4]"")");
        Assert.AreEqual(sheet.GetCell(5, 1).Formula, @"SUM(""R6C1:R6C[5]"")");
        Assert.AreEqual(sheet.GetCell(7, 1).Formula, @"SUM(""R8C[6]:R8C[8]"")");
    }
    
    [Test]
    public void OnRowDelete_AbsoluteAddresses_Test()
    {
        // Arrange
        var sheet = GetSheet();

        // value cells
        sheet.GetCell(1, 3).Value = 42;
        sheet.GetCell(7, 1).Value = 100;
        
        // formula cells
        sheet.GetCell(1, 1).Formula = @"VAL(""R7C1"")";
        sheet.GetCell(1, 2).Formula = @"SUM(""R7C1:R7C1"")+VAL(""R7C1"")";
        sheet.GetCell(1, 5).Formula = @"SUM(""R1C6:R8C6"")";
        sheet.GetCell(7, 3).Formula = @"VAL(""R7C1"")";
        

        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        IFormulaShifter formulaShifter = new CellShiftFormulaHelper(sheet);
        var shiftLog = formulaShifter.OnRowDelete(5);

        // Assert
        Assert.AreEqual(sheet.GetCell(1, 1).Formula, @"VAL(""R6C1"")");
        Assert.AreEqual(sheet.GetCell(1, 2).Formula, @"SUM(""R6C1:R6C1"")+VAL(""R6C1"")");
        Assert.AreEqual(sheet.GetCell(1, 5).Formula, @"SUM(""R1C6:R7C6"")");
        Assert.AreEqual(sheet.GetCell(7, 3).Formula, @"VAL(""R6C1"")");
    }
    
    [Test]
    public void OnRowDelete_RelativeAddresses_Test()
    {
        // Arrange
        var sheet = GetSheet();

        // value cells
        sheet.GetCell(4, 1).Value = 100;
        sheet.GetCell(6, 3).Value = 42;
        
        // formula cells
        sheet.GetCell(1, 1).Formula = @"SUM(""R1C5:R[6]C5"")";
        sheet.GetCell(4, 3).Formula = @"VAL(""R[3]C3"")";
        sheet.GetCell(7, 1).Formula = @"VAL(""R[-3]C1"")";

        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        IFormulaShifter formulaShifter = new CellShiftFormulaHelper(sheet);
        var shiftLog = formulaShifter.OnRowDelete(5);

        // Assert
        Assert.AreEqual(sheet.GetCell(1, 1).Formula, @"SUM(""R1C5:R[5]C5"")");
        Assert.AreEqual(sheet.GetCell(4, 3).Formula, @"VAL(""R[2]C3"")");
        Assert.AreEqual(sheet.GetCell(7, 1).Formula, @"VAL(""R[-2]C1"")");
    }

    [Test]
    public void OnColumnDelete_AbsoluteAddresses_Test()
    {
        // Arrange
        var sheet = GetSheet();

        // value cells
        sheet.GetCell(1, 4).Value = 100;
        sheet.GetCell(3, 1).Value = 42;
        
        // formula cells
        sheet.GetCell(1, 1).Formula = @"VAL(""R1C4"")";
        sheet.GetCell(3, 4).Formula = @"VAL(""R3C1"")";
        sheet.GetCell(5, 1).Formula = @"SUM(""R6C1:R6C5"")";
        sheet.GetCell(6, 1).Formula = @"SUM(""R7C5:R7C7"")";

        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        IFormulaShifter formulaShifter = new CellShiftFormulaHelper(sheet);
        var shiftLog = formulaShifter.OnColumnDelete(3);

        // Assert
        Assert.AreEqual(sheet.GetCell(1, 1).Formula, @"VAL(""R1C3"")");
        Assert.AreEqual(sheet.GetCell(3, 4).Formula, @"VAL(""R3C1"")");
        Assert.AreEqual(sheet.GetCell(5, 1).Formula, @"SUM(""R6C1:R6C4"")");
        Assert.AreEqual(sheet.GetCell(6, 1).Formula, @"SUM(""R7C4:R7C6"")");
    }
    
    [Test]
    public void OnColumnDelete_RelativeAddresses_Test()
    {
        // Arrange
        var sheet = GetSheet();

        // value cells
        sheet.GetCell(1, 4).Value = 100;
        sheet.GetCell(3, 1).Value = 42;
        
        // formula cells
        sheet.GetCell(1, 1).Formula = @"VAL(""R1C[4]"")";
        sheet.GetCell(3, 4).Formula = @"VAL(""R3C[-4]"")";
        sheet.GetCell(5, 1).Formula = @"SUM(""R6C1:R6C[4]"")";
        sheet.GetCell(7, 1).Formula = @"SUM(""R8C[4]:R8C[6]"")";
        
        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        IFormulaShifter formulaShifter = new CellShiftFormulaHelper(sheet);
        var shiftLog = formulaShifter.OnColumnDelete(3);

        // Assert
        Assert.AreEqual(sheet.GetCell(1, 1).Formula, @"VAL(""R1C[3]"")");
        Assert.AreEqual(sheet.GetCell(3, 4).Formula, @"VAL(""R3C[-3]"")");
        Assert.AreEqual(sheet.GetCell(5, 1).Formula, @"SUM(""R6C1:R6C[3]"")");
        Assert.AreEqual(sheet.GetCell(7, 1).Formula, @"SUM(""R8C[3]:R8C[5]"")");
    }

    [Test]
    public void ColumnTotals_Test()
    {
        // Arrange
        var sheet = GetSheet();

        // value cells
        sheet.GetCell(1, 1).Value = 1;
        sheet.GetCell(1, 2).Value = 1;
        sheet.GetCell(1, 3).Value = 1;
        
        sheet.GetCell(2, 1).Value = 2;
        sheet.GetCell(2, 2).Value = 2;
        sheet.GetCell(2, 3).Value = 2;
        
        sheet.GetCell(3, 1).Value = 3;
        sheet.GetCell(3, 2).Value = 3;
        sheet.GetCell(3, 3).Value = 3;
        
        // formula cells
        sheet.GetCell(1, 4).Formula = @"SUM(""R1C1:R1C3"")";
        sheet.GetCell(2, 4).Formula = @"SUM(""R2C1:R2C3"")";
        sheet.GetCell(3, 4).Formula = @"SUM(""R3C1:R3C3"")";
        
        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        var insertedRowNumber = 2;
        var row = sheet.GetRow(insertedRowNumber);
        sheet.AddRow(row, 1, true);
        
        IFormulaShifter formulaShifter = new CellShiftFormulaHelper(sheet);
        formulaShifter.OnRowAdd(insertedRowNumber);

        // Assert
        Assert.AreEqual(sheet.GetCell(1, 4).Formula, @"SUM(""R1C1:R1C3"")");
        Assert.AreEqual(sheet.GetCell(2, 4).Formula, @"SUM(""R2C1:R2C3"")");
        Assert.AreEqual(sheet.GetCell(3, 4).Formula, @"SUM(""R3C1:R3C3"")");
        Assert.AreEqual(sheet.GetCell(4, 4).Formula, @"SUM(""R4C1:R4C3"")");
    }
    
    [Test]
    public void ColumnTotals_2_Test()
    {
        // Arrange
        var sheet = GetSheet();

        // value cells
        sheet.GetCell(1, 1).Value = 1;
        sheet.GetCell(1, 2).Value = 1;
        sheet.GetCell(1, 3).Value = 1;
        
        sheet.GetCell(2, 1).Value = 2;
        sheet.GetCell(2, 2).Value = 2;
        sheet.GetCell(2, 3).Value = 2;
        
        sheet.GetCell(3, 1).Value = 3;
        sheet.GetCell(3, 2).Value = 3;
        sheet.GetCell(3, 3).Value = 3;
        
        // formula cells
        sheet.GetCell(1, 4).Formula = @"SUM(""RC1:RC3"")";
        sheet.GetCell(2, 4).Formula = @"SUM(""RC1:RC3"")";
        sheet.GetCell(3, 4).Formula = @"SUM(""RC1:RC3"")";
        
        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        var insertedRowNumber = 2;
        var row = sheet.GetRow(insertedRowNumber);
        sheet.AddRow(row, 1, true);
        
        IFormulaShifter formulaShifter = new CellShiftFormulaHelper(sheet);
        formulaShifter.OnRowAdd(insertedRowNumber);

        // Assert
        Assert.AreEqual(sheet.GetCell(1, 4).Formula, @"SUM(""RC1:RC3"")");
        Assert.AreEqual(sheet.GetCell(2, 4).Formula, @"SUM(""RC1:RC3"")");
        Assert.AreEqual(sheet.GetCell(3, 4).Formula, @"SUM(""RC1:RC3"")");
        Assert.AreEqual(sheet.GetCell(4, 4).Formula, @"SUM(""RC1:RC3"")");
    }
    
    
    #region private methods
    private Sheet GetSheet()
    {
        var sheetSettings = new SheetSettings 
        {
            RowsCount = 10,
            ColumnsCount = 10
        };

        var sheet = new Sheet(sheetSettings)
        {
            Name = "main"
        };

        return sheet;
    }
    #endregion
}