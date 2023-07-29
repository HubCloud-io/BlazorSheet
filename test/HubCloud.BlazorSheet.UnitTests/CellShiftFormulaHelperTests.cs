using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class CellShiftFormulaHelperTests
{
    [Test]
    public void OnRowAdd_SimpleAddresses_Test()
    {
        var workbook = BuildTestWorkbook();
        var sheet = workbook.FirstSheet;
        
        var shiftFormulaHelper = new CellShiftFormulaHelper(sheet);
        var shiftLog = shiftFormulaHelper.OnRowAdd(5);

        var formula11 = sheet.GetCell(1, 1).Formula;
        var formula63 = sheet.GetCell(6, 3).Formula;
        var formula12 = sheet.GetCell(1, 2).Formula;
        
        Assert.AreEqual(shiftLog.Count, 3);
        Assert.AreEqual(formula11, @"VAL(""R7C1"")");
        Assert.AreEqual(formula63, @"VAL(""R7C1"")");
        Assert.AreEqual(formula12, @"SUM(""R7C1:R7C1"")");
    }
    
    #region private methods
    private Workbook BuildTestWorkbook()
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

        sheet.GetCell(6, 1).Value = 100;
        sheet.GetCell(1, 1).Formula = @"VAL(""R6C1"")";
        sheet.GetCell(6, 3).Formula = @"VAL(""R6C1"")";
        sheet.GetCell(1, 2).Formula = @"SUM(""R6C1:R6C1"")";
        sheet.GetCell(6, 5).Formula = @"VAL(""R1C3"")";

        var workbook = new Workbook();
        workbook.AddSheet(sheet);

        return workbook;
    }
    #endregion
}