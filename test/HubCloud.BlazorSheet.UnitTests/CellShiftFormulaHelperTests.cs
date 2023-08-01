using HubCloud.BlazorSheet.Core.Enums;
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
        IFormulaShifter formulaShifter = new CellShiftFormulaHelper2(sheet);
        var shiftLog = formulaShifter.OnRowAdd(5);

        // Assert
        var formula11 = sheet.GetCell(1, 1).Formula;
        var formula63 = sheet.GetCell(6, 3).Formula;
        var formula12 = sheet.GetCell(1, 2).Formula;
        var formula15 = sheet.GetCell(1, 5).Formula;
        
        Assert.AreEqual(formula11, @"VAL(""R7C1"")");
        Assert.AreEqual(formula63, @"VAL(""R7C1"")");
        Assert.AreEqual(formula12, @"SUM(""R7C1:R7C1"")+VAL(""R7C1"")");
        Assert.AreEqual(formula15, @"SUM(""R1C6:R8C6"")");
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
        IFormulaShifter formulaShifter = new CellShiftFormulaHelper2(sheet);
        var shiftLog = formulaShifter.OnRowAdd(5);

        // Assert
        var formula61 = sheet.GetCell(6, 1).Formula;
        var formula43 = sheet.GetCell(4, 3).Formula;
        var formula11 = sheet.GetCell(1, 1).Formula;

        Assert.AreEqual(formula61, @"VAL(""R[-3]C1"")");
        Assert.AreEqual(formula43, @"VAL(""R[3]C3"")");
        Assert.AreEqual(formula11, @"SUM(""R1C5:R[6]C5"")");
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