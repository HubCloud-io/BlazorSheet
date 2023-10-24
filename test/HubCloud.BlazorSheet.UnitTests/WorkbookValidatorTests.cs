using System;
using System.Linq;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.ExamplesShared.WorkbookBuilders;
using HubCloud.BlazorSheet.Validator;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class WorkbookValidatorTests
{
    private Workbook _workbook;

    [SetUp]
    public void Setup()
    {
        _workbook = BuildWorkbook();
        
        // first sheet
        _workbook.Sheets.First().EditSettings.ForEach(x => x.Required = true);
        _workbook.Sheets.First().GetCell(5, 3).Value = 0;
        _workbook.Sheets.First().GetCell(5, 4).Value = 1;
        _workbook.Sheets.First().GetCell(5, 5).Value = 2;
        _workbook.Sheets.First().GetCell(6, 3).Value = 3;
        _workbook.Sheets.First().GetCell(6, 4).Value = 4;
        _workbook.Sheets.First().GetCell(6, 5).Value = 5;
        _workbook.Sheets.First().GetCell(7, 3).Value = 6;
        _workbook.Sheets.First().GetCell(7, 4).Value = 7;
        _workbook.Sheets.First().GetCell(7, 5).Value = 8;
        
        // second sheet
        _workbook.Sheets.Last().EditSettings.ForEach(x => x.Required = true);
        _workbook.Sheets.Last().GetCell(2, 3).Value = DateTime.Now;
        _workbook.Sheets.Last().GetCell(5, 3).Value = 10;
        _workbook.Sheets.Last().GetCell(5, 4).Value = 1;
        _workbook.Sheets.Last().GetCell(5, 5).Value = 2;
        _workbook.Sheets.Last().GetCell(6, 3).Value = 0;
        _workbook.Sheets.Last().GetCell(6, 4).Value = 4;
        _workbook.Sheets.Last().GetCell(6, 5).Value = 5;
        _workbook.Sheets.Last().GetCell(7, 3).Value = 6;
        _workbook.Sheets.Last().GetCell(7, 4).Value = 7;
        _workbook.Sheets.Last().GetCell(7, 5).Value = 8;
    }

    [Test]
    public void SimpleSheetValidationTest()
    {
        // Arrange
        var sheet = _workbook.FirstSheet;
        
        // Act
        var validator = new WorkbookValidator();
        var validationResults = validator.Validate(sheet)
            .ToArray();

        // Assert
        Assert.AreEqual(validationResults.Count(), 2);
        Assert.AreEqual(validationResults[0].CellAddress, "R2C3");
        Assert.AreEqual(validationResults[0].SheetName, "first_sheet");
        Assert.AreEqual(validationResults[0].SheetUid, _workbook.FirstSheet.Uid);
        
        Assert.AreEqual(validationResults[1].CellAddress, "R5C3");
        Assert.AreEqual(validationResults[1].SheetName, "first_sheet");
        Assert.AreEqual(validationResults[1].SheetUid, _workbook.FirstSheet.Uid);
    }

    [Test]
    public void SimpleWorkbookValidationTest()
    {
        // Act
        var validator = new WorkbookValidator();
        var validationResults = validator.Validate(_workbook)
            .ToArray();

        // Assert
        Assert.AreEqual(validationResults.Count(), 3);
        Assert.AreEqual(validationResults[0].CellAddress, "R2C3");
        Assert.AreEqual(validationResults[0].SheetName, "first_sheet");
        Assert.AreEqual(validationResults[0].SheetUid, _workbook.FirstSheet.Uid);
        
        Assert.AreEqual(validationResults[1].CellAddress, "R5C3");
        Assert.AreEqual(validationResults[1].SheetName, "first_sheet");
        Assert.AreEqual(validationResults[1].SheetUid, _workbook.FirstSheet.Uid);
        
        Assert.AreEqual(validationResults[2].CellAddress, "R6C3");
        Assert.AreEqual(validationResults[2].SheetName, "second_sheet");
        Assert.AreEqual(validationResults[2].SheetUid, _workbook.Sheets.Last().Uid);
    }

    private Workbook BuildWorkbook()
    {
        var sheetSettings = new SheetSettings();
        sheetSettings.RowsCount = 10;
        sheetSettings.ColumnsCount = 6;

        var editSettings = new SheetCellEditSettings()
        {
            ControlKind = CellControlKinds.NumberInput,
            NumberDigits = 2
        };

        sheetSettings.EditSettings.Add(editSettings);

        var editSettingsDate = new SheetCellEditSettings()
        {
            ControlKind = CellControlKinds.DateInput
        };
        sheetSettings.EditSettings.Add(editSettingsDate);

        var totalStyle = new SheetCellStyle()
        {
            FontWeight = "bold",
            FontSize = "14px",
            TextAlign = "right"
        };
        sheetSettings.Styles.Add(totalStyle);

        var numberEditCellStyle = new SheetCellStyle()
        {
            TextAlign = "right",
            BackgroundColor = "#f2fff4"
        };
        sheetSettings.Styles.Add(numberEditCellStyle);

        var otherEditCellStyle = new SheetCellStyle()
        {
            BackgroundColor = "#f2fff4"
        };
        sheetSettings.Styles.Add(otherEditCellStyle);

        var firstSheet = new Sheet(sheetSettings);
        firstSheet.Name = "first_sheet";
        firstSheet.Uid = Guid.NewGuid();

        firstSheet.GetCell(1, 3).Value = "Start date";
        firstSheet.GetCell(2, 3).EditSettingsUid = editSettingsDate.Uid;
        firstSheet.GetCell(2, 3).StyleUid = otherEditCellStyle.Uid;
        firstSheet.GetCell(2, 3).Format = "dd.MM.yyyy";

        firstSheet.GetCell(4, 2).Value = "Budget item / Period";

        firstSheet.GetCell(4, 3).Formula = $"Val(\"R2C3\").BeginYear()";
        firstSheet.GetCell(4, 3).Format = "dd.MM.yyyy";

        firstSheet.GetCell(4, 4).Formula = $"Val(\"R2C3\").BeginYear().AddMonths(1)";
        firstSheet.GetCell(4, 4).Format = "dd.MM.yyyy";
        firstSheet.GetCell(4, 5).Formula = $"Val(\"R2C3\").BeginYear().AddMonths(2)";
        firstSheet.GetCell(4, 5).Format = "dd.MM.yyyy";

        firstSheet.GetCell(5, 2).Value = "Rent";
        firstSheet.GetCell(6, 2).Value = "Tax";
        firstSheet.GetCell(7, 2).Value = "Salary";

        for (var r = 5; r <= 7; r++)
        {
            var row = firstSheet.GetRow(r);
            row.IsAddRemoveAllowed = true;
            for (var c = 3; c <= 5; c++)
            {
                var currentCell = firstSheet.GetCell(r, c);
                currentCell.EditSettingsUid = editSettings.Uid;
                currentCell.Format = "F2";
                currentCell.StyleUid = numberEditCellStyle.Uid;
            }
        }

        firstSheet.GetCell(8, 2).StyleUid = totalStyle.Uid;
        firstSheet.GetCell(8, 2).Value = "Total";

        firstSheet.GetCell(4, 6).Value = "Total";
        firstSheet.GetCell(4, 6).StyleUid = totalStyle.Uid;

        firstSheet.GetCell(8, 6).Formula = $"Sum(\"R5C6:R[-1]C6\")";
        firstSheet.GetCell(8, 6).StyleUid = totalStyle.Uid;

        firstSheet.GetCell(1, 6).Value = "About Blazor";
        firstSheet.GetCell(1, 6).Link = "https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor";

        firstSheet.PrepareCellText();

        var column = firstSheet.GetColumn(1);
        column.WidthValue = 40;

        column = firstSheet.GetColumn(2);
        column.WidthValue = 200;

        firstSheet.GetColumn(3).WidthValue = 120;
        firstSheet.GetColumn(4).WidthValue = 120;
        firstSheet.GetColumn(5).WidthValue = 120;

        var workbook = new Workbook();
        workbook.AddSheet(firstSheet);
        
        var secondSheet = new Sheet(sheetSettings);
        secondSheet.Name = "second_sheet";
        secondSheet.Uid = Guid.NewGuid();

        secondSheet.GetCell(1, 3).Value = "Start date";
        secondSheet.GetCell(2, 3).EditSettingsUid = editSettingsDate.Uid;
        secondSheet.GetCell(2, 3).StyleUid = otherEditCellStyle.Uid;
        secondSheet.GetCell(2, 3).Format = "dd.MM.yyyy";

        secondSheet.GetCell(4, 2).Value = "Budget item / Period";

        secondSheet.GetCell(4, 3).Formula = $"Val(\"R2C3\").BeginYear()";
        secondSheet.GetCell(4, 3).Format = "dd.MM.yyyy";

        secondSheet.GetCell(4, 4).Formula = $"Val(\"R2C3\").BeginYear().AddMonths(1)";
        secondSheet.GetCell(4, 4).Format = "dd.MM.yyyy";
        secondSheet.GetCell(4, 5).Formula = $"Val(\"R2C3\").BeginYear().AddMonths(2)";
        secondSheet.GetCell(4, 5).Format = "dd.MM.yyyy";

        secondSheet.GetCell(5, 2).Value = "Rent";
        secondSheet.GetCell(6, 2).Value = "Tax";
        secondSheet.GetCell(7, 2).Value = "Salary";

        for (var r = 5; r <= 7; r++)
        {
            var row = secondSheet.GetRow(r);
            row.IsAddRemoveAllowed = true;
            for (var c = 3; c <= 5; c++)
            {
                var currentCell = secondSheet.GetCell(r, c);
                currentCell.EditSettingsUid = editSettings.Uid;
                currentCell.Format = "F2";
                currentCell.StyleUid = numberEditCellStyle.Uid;
            }
        }

        secondSheet.GetCell(8, 2).StyleUid = totalStyle.Uid;
        secondSheet.GetCell(8, 2).Value = "Total";

        secondSheet.GetCell(4, 6).Value = "Total";
        secondSheet.GetCell(4, 6).StyleUid = totalStyle.Uid;

        secondSheet.GetCell(8, 6).Formula = $"Sum(\"R5C6:R[-1]C6\")";
        secondSheet.GetCell(8, 6).StyleUid = totalStyle.Uid;

        secondSheet.GetCell(1, 6).Value = "About Blazor";
        secondSheet.GetCell(1, 6).Link = "https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor";

        secondSheet.PrepareCellText();

        var column1 = secondSheet.GetColumn(1);
        column1.WidthValue = 40;

        column1 = secondSheet.GetColumn(2);
        column1.WidthValue = 200;

        secondSheet.GetColumn(3).WidthValue = 120;
        secondSheet.GetColumn(4).WidthValue = 120;
        secondSheet.GetColumn(5).WidthValue = 120;

        workbook.AddSheet(secondSheet);

        return workbook;
    }
}