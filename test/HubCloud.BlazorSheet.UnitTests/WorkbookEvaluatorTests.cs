using System.Security.Cryptography;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine;
using HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class WorkbookEvaluatorTests
{
    private Workbook _workbookFormulas;
    
    [SetUp]
    public void Setup()
    {
        var builder = new WorkbookFormulaExamplesBuilder();
        _workbookFormulas = builder.Build();
    }

    // Write tests here.
    
    [Test]
    public void Add_ReturnResult()
    {
        var evaluator = new WorkbookEvaluator(_workbookFormulas);
        evaluator.EvalWorkbook();

        var result = _workbookFormulas.FirstSheet.GetCell(2, 5).Value;
        
        Assert.AreEqual(4m,result);
    }

    [Test]
    public void CalcFullWorkbook_Test()
    {
        // Arrange
        var sheetSettings = new SheetSettings
        {
            RowsCount = 10,
            ColumnsCount = 5
        };
        
        var sheet = new Sheet(sheetSettings)
        {
            Name = "main"
        };
        
        // const values
        sheet.GetCell(1, 1).Value = 42;
        sheet.GetCell(9, 1).Value = 100;

        // first branch
        sheet.GetCell(1, 2).Formula = @"VAL(""R1C1"") + 10";
        sheet.GetCell(3, 2).Formula = @"VAL(""R1C1"")";
        
        sheet.GetCell(1, 3).Formula = @"VAL(""R1C2"") + 1";
        sheet.GetCell(2, 3).Formula = @"VAL(""R1C2"") + VAL(""R9C4"")";
        
        // second branch
        sheet.GetCell(9, 2).Formula = @"VAL(""R9C1"") + 1";
        sheet.GetCell(9, 3).Formula = @"VAL(""R9C2"") + 9";
        sheet.GetCell(9, 4).Formula = @"VAL(""R9C3"") + 10";
        
        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        var evaluator = new WorkbookEvaluator(workbook);
        evaluator.EvalWorkbook();
        
        // Assert
        // first branch
        var cell = sheet.GetCell(1, 2);
        Assert.AreEqual(cell.Value, 52);
        cell = sheet.GetCell(3, 2);
        Assert.AreEqual(cell.Value, 42);
        cell = sheet.GetCell(1, 3);
        Assert.AreEqual(cell.Value, 53);
        cell = sheet.GetCell(2, 3);
        Assert.AreEqual(cell.Value, 172);
        
        // second branch
        cell = sheet.GetCell(9, 2);
        Assert.AreEqual(cell.Value, 101);
        cell = sheet.GetCell(9, 3);
        Assert.AreEqual(cell.Value, 110);
        cell = sheet.GetCell(9, 4);
        Assert.AreEqual(cell.Value, 120);
    }

    
    // R1C1
    //     R1C2
    //         R1C3
    //         R2C3
    //     R3C2
    [Test]
    public void CalcFromOnceCell_Test()
    {
        // Arrange
        var sheetSettings = new SheetSettings
        {
            RowsCount = 10,
            ColumnsCount = 5
        };
        
        var sheet = new Sheet(sheetSettings)
        {
            Name = "main"
        };
        
        // const values
        sheet.GetCell(1, 1).Value = 42;
        sheet.GetCell(9, 1).Value = 100;

        // first branch
        sheet.GetCell(1, 2).Formula = @"VAL(""R1C1"") + 10";
        sheet.GetCell(3, 2).Formula = @"VAL(""R1C1"")";
        
        sheet.GetCell(1, 3).Formula = @"VAL(""R1C2"") + 1";
        sheet.GetCell(2, 3).Formula = @"VAL(""R1C2"") + VAL(""R9C4"")";
        
        // second branch
        sheet.GetCell(9, 2).Formula = @"VAL(""R9C1"") + 1";
        sheet.GetCell(9, 3).Formula = @"VAL(""R9C2"") + 9";
        sheet.GetCell(9, 4).Formula = @"VAL(""R9C3"") + 10";
        
        var workbook = new Workbook();
        workbook.AddSheet(sheet);
        
        // Act
        var evaluator = new WorkbookEvaluator(workbook);
        evaluator.EvalWorkbook(new SheetCellAddress
        {
            Row = 1,
            Column = 1
        });
        
        // Assert
        // level 1
        var cell = sheet.GetCell(1, 1);
        Assert.AreEqual(cell.Value, 42);
        
        // level 2
        cell = sheet.GetCell(1, 2);
        Assert.AreEqual(cell.Value, 52);
        cell = sheet.GetCell(3, 2);
        Assert.AreEqual(cell.Value, 42);
        
        // level 3
        cell = sheet.GetCell(1, 3);
        Assert.AreEqual(cell.Value, 53);
        cell = sheet.GetCell(2, 3);
        Assert.AreEqual(cell.Value, 172);
    }
}