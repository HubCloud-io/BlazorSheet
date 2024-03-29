﻿using System.Collections.Generic;
using System.Linq;
using HubCloud.BlazorSheet.Core.Models;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class SheetTests
{
    [TestCase(1, 1)]
    [TestCase(0, 10)]
    [TestCase(-1, 10)]
    public void RowsCount_SetValue_ReturnValue(int input, int check)
    {
        var sheet = new Sheet();
        sheet.RowsCount = input;

        Assert.AreEqual(check, sheet.RowsCount);
    }

    [TestCase(1, 1)]
    [TestCase(0, 10)]
    [TestCase(-1, 10)]
    public void ColumnsCount_SetValue_ReturnValue(int input, int check)
    {
        var sheet = new Sheet();
        sheet.ColumnsCount = input;

        Assert.AreEqual(check, sheet.ColumnsCount);
    }

    [TestCase(1, 1, "R1C1")]
    [TestCase(3, 3, "R3C3")]
    [TestCase(1, 3, "R1C3")]
    [TestCase(3, 1, "R3C1")]
    [TestCase(2, 2, "R2C2")]
    public void GetCell_ExistingCell_ReturnCell(int rowNumber, int columnNumber, string check)
    {
        var sheetSettings = BuildSheetSettingsWithCellNames(3, 3);

        var sheet = new Sheet(sheetSettings);

        var cell = sheet.GetCell(rowNumber, columnNumber);
        Assert.IsNotNull(cell);
        Assert.AreEqual(check, cell.Value);
    }

    [TestCase(1, 4)]
    [TestCase(0, 0)]
    [TestCase(1, 0)]
    public void GetCell_NotExistingCell_ReturnNull(int rowNumber, int columnNumber)
    {
        var sheetSettings = BuildSheetSettingsWithCellNames(3, 3);

        var sheet = new Sheet(sheetSettings);

        var cell = sheet.GetCell(rowNumber, columnNumber);
        Assert.IsNull(cell);
    }

    [TestCase(1, -1, 1, 1)]
    [TestCase(1, 1, 2, 1)]
    [TestCase(3, 1, 4, 1)]
    public void AddRow(int baseRowNumber, int position, int checkRow, int checkColumn)
    {
        var sheetSettings = BuildSheetSettingsWithCellNames(3, 3);
        var sheet = new Sheet(sheetSettings);

        var firstRow = sheet.Rows.ToList()[baseRowNumber - 1];
        sheet.AddRow(firstRow, position, false);

        var cellTopLeft = sheet.GetCell(checkRow, checkColumn);

        Assert.AreEqual(4, sheet.RowsCount);
        Assert.AreEqual(3, sheet.ColumnsCount);
        Assert.AreEqual(12, sheet.Cells.Count);
        Assert.IsNull(cellTopLeft.Value);
    }

    [TestCase(1, -1, 1, 1)]
    [TestCase(1, 1, 1, 2)]
    [TestCase(3, 1, 1, 4)]
    public void AddColumn(int baseColumnNumber, int position, int checkRow, int checkColumn)
    {
        var sheetSettings = BuildSheetSettingsWithCellNames(3, 3);
        var sheet = new Sheet(sheetSettings);

        var firstColumn = sheet.Columns.ToList()[baseColumnNumber - 1];
        sheet.AddColumn(firstColumn, position, false);

        var checkCell = sheet.GetCell(checkRow, checkColumn);

        Assert.AreEqual(3, sheet.RowsCount);
        Assert.AreEqual(4, sheet.ColumnsCount);
        Assert.AreEqual(12, sheet.Cells.Count);
        Assert.IsNull(checkCell.Value);
    }

    [Test]
    public void SetStyle_DuplicateStyles_UniqueStyles()
    {
        var sheetSettings = BuildSheetSettingsWithCellNames(3, 3);
        var sheet = new Sheet(sheetSettings);

        var redStyle = new SheetCellStyle()
        {
            BackgroundColor = "red"
        };
        var redStyleDuplicate = new SheetCellStyle()
        {
            BackgroundColor = "red"
        };
        var greenStyle = new SheetCellStyle()
        {
            BackgroundColor = "green"
        };

        sheet.SetStyle(sheet.GetCell(1, 1), redStyle);
        sheet.SetStyle(sheet.GetCell(1, 2), redStyleDuplicate);
        sheet.SetStyle(sheet.GetCell(1, 3), greenStyle);

        Assert.AreEqual(2, sheet.Styles.Count);
        Assert.AreEqual(redStyle.Uid, sheet.GetCell(1, 1).StyleUid);
        Assert.AreEqual(redStyle.Uid, sheet.GetCell(1, 2).StyleUid);
        Assert.AreEqual(greenStyle.Uid, sheet.GetCell(1, 3).StyleUid);
    }

    [TestCase(10, 10, 10, 10)]
    [TestCase(5, 5, 5, 5)]
    [TestCase(0, 0, 5, 5)]
    [TestCase(-1, -1, 5, 5)]
    [TestCase(1, 1, 1, 1)]
    public void ChangeSize(int newColumnsCount, int newRowsCount, int checkColumnsCount, int checkRowsCount)
    {
        var sheetSettings = BuildSheetSettingsWithCellNames(5, 5);

        var sheet = new Sheet(sheetSettings);

        sheet.ChangeSize(newColumnsCount, newRowsCount);

        Assert.AreEqual(checkColumnsCount, sheet.Columns.Count);
        Assert.AreEqual(checkRowsCount, sheet.Rows.Count);
    }

    [TestCase(7, 1, 7, 9, 9)]
    [TestCase(1, 1, 9, 1, 9)]
    [TestCase(1, 1, 2, 2, 4)]
    [TestCase(0, 1, 2, 2, 0)]
    [TestCase(1, 0, 2, 2, 0)]
    [TestCase(1, 1, 0, 2, 0)]
    [TestCase(1, 1, 2, 0, 0)]
    [TestCase(2, 2, 6, 6, 25)]
    [TestCase(2, 2, 3, 4, 6)]
    [TestCase(1, 2, 1, 4, 3)]
    public void GetCellsByRange_ReturnListCells(int fromRow, int fromCol, int toRow, int toCol, int expectedCount)
    {
        var sheet = new Sheet(10, 10);
        var cells = sheet.GetCellsByRange(fromRow, fromCol, toRow, toCol);

        Assert.AreEqual(expectedCount, cells.Count);
    }

    [Test]
    public void GetDimensionEndRowNumber_ReturnsLastFillRow()
    {
        var sheetSettings = BuildSheetSettingsWithCellNames(5, 5);
        var sheet = new Sheet(sheetSettings);

        sheet.AddRow(sheet.Rows.Last(), 1, false);
        sheet.AddRow(sheet.Rows.Last(), 1, false);
        sheet.AddRow(sheet.Rows.Last(), 1, false);

        var rowNumber = sheet.GetDimensionEndRowNumber();

        Assert.AreEqual(5, rowNumber);
    }

    [Test]
    public void ApplyStyleParams()
    {
        var sheet = new Sheet(5, 5);

        var cell11 = sheet.GetCell(1, 1);
        var cell12 = sheet.GetCell(1, 2);

        var style = new SheetCellStyle
        {
            BackgroundColor = "red",
            BorderBottom = "1px solid #275081",
            BorderLeft = "1px solid #275081",
            BorderRight = "1px solid #275081",
            BorderTop = "1px solid #275081",
            Color = "#ffffff",
            FontSize = "12px",
            FontStyle = "italic",
            FontWeight = "bold",
            TextAlign = "center"
        };

        sheet.SetStyle(cell11, style);
        sheet.SetStyle(cell12, style);

        var styleParams = new Dictionary<string, object>
        {
            { nameof(SheetCellStyle.TextAlign), "left" },
            { nameof(SheetCellStyle.BackgroundColor), "green" }
        };
        sheet.ApplyStyleParams(new List<SheetCell> { cell11 }, styleParams);

        var style1 = sheet.GetStyle(cell11);
        var style2 = sheet.GetStyle(cell12);

        var result = style1.IsStyleEqual(style2);

        Assert.AreEqual(false, result);
    }

    private SheetSettings BuildSheetSettingsWithCellNames(int rowsCount, int columnsCount)
    {
        var sheetSettings = new SheetSettings()
        {
            RowsCount = rowsCount,
            ColumnsCount = columnsCount
        };

        for (var r = 1; r <= rowsCount; r++)
        {
            var newRow = new SheetRow();
            for (var c = 1; c <= columnsCount; c++)
            {
                SheetColumn column;
                if (r == 1)
                {
                    column = new SheetColumn();
                    sheetSettings.Columns.Add(column);
                }
                else
                {
                    column = sheetSettings.Columns[c - 1];
                }

                var newCell = new SheetCell()
                {
                    RowUid = newRow.Uid,
                    ColumnUid = column.Uid,
                    Value = $"R{r}C{c}"
                };
                sheetSettings.Cells.Add(newCell);
            }

            sheetSettings.Rows.Add(newRow);
        }

        return sheetSettings;
    }
}