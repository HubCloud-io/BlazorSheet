using HubCloud.BlazorSheet.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubCloud.BlazorSheet.UnitTests
{
    [TestFixture]
    public class SheetMergeCellsTests
    {
        [Test]
        public void CanCellsBeJoined_SelectedCellsWithGaps_ReturnEqualFalse()
        {
            var sheetSettings = BuildSheetSettingsWithCellNames(10, 10);
            var sheet = new Sheet(sheetSettings);

            var selectedCells = sheet.Cells.Take(2).ToList();
            selectedCells.AddRange(sheet.Cells.Skip(4).Take(2));

            var result = sheet.CanCellsBeJoined(selectedCells.ToList());

            Assert.IsFalse(result);
        }

        [Test]
        public void CanCellsBeJoined_SelectedCellsWithoutGaps_ReturnEqualTrue()
        {
            var sheetSettings = BuildSheetSettingsWithCellNames(10, 10);
            var sheet = new Sheet(sheetSettings);

            var selectedCells = sheet.Cells.Take(4).ToList();

            var result = sheet.CanCellsBeJoined(selectedCells.ToList());

            Assert.IsTrue(result);
        }

        [TestCase(1, 5, 1, 5, 5, 5)]
        [TestCase(1, 1, 1, 5, 1, 5)]
        [TestCase(1, 5, 1, 1, 5, 1)]
        public void JoinCells_ReturnTopLeftCell_CorrectColspanRowspan(
            int firstColumnNumber,
            int lastColumnNumber,
            int firstRowNumber,
            int lastRowNumber,
            int topLeftCellColspanResult,
            int topLeftCellRowspanResult)
        {
            var sheetSettings = BuildSheetSettingsWithCellNames(10, 10);
            var sheet = new Sheet(sheetSettings);
            var cellWithAddressList = new List<CellWithAddress>();

            sheet.Cells.ToList().ForEach(x => cellWithAddressList.Add(new CellWithAddress
            {
                Cell = x,
                Address = sheet.CellAddress(x)
            }));

            var selectedCells = cellWithAddressList
                .Where(x => x.Address.Column >= firstColumnNumber && x.Address.Column <= lastColumnNumber &&
                            x.Address.Row >= firstRowNumber && x.Address.Row <= lastRowNumber)
                .Select(s => s.Cell)
                .ToList();

            var result = sheet.JoinCells(selectedCells);

            Assert.AreEqual(topLeftCellColspanResult, result.Colspan);
            Assert.AreEqual(topLeftCellRowspanResult, result.Rowspan);
        }

        [TestCase(1, 5, 1, 5)]
        [TestCase(1, 1, 1, 5)]
        [TestCase(1, 5, 1, 1)]
        public void JoinCells_ReturnTopLeftCell_CellsHiddenByJoinEqualTrue(
            int firstColumnNumber,
            int lastColumnNumber,
            int firstRowNumber,
            int lastRowNumber)
        {
            var sheetSettings = BuildSheetSettingsWithCellNames(10, 10);
            var sheet = new Sheet(sheetSettings);
            var cellWithAddressList = new List<CellWithAddress>();

            sheet.Cells.ToList().ForEach(x => cellWithAddressList.Add(new CellWithAddress
            {
                Cell = x,
                Address = sheet.CellAddress(x)
            }));

            var selectedCells = cellWithAddressList
                .Where(x => x.Address.Column >= firstColumnNumber && x.Address.Column <= lastColumnNumber &&
                            x.Address.Row >= firstRowNumber && x.Address.Row <= lastRowNumber)
                .Select(s => s.Cell)
                .ToList();

            var topLeftCell = sheet.JoinCells(selectedCells);

            var result = selectedCells
                .Where(x => x.Uid != topLeftCell.Uid)
                .All(x => x.HiddenByJoin == true);

            Assert.IsTrue(result);
        }

        [Test]
        public void JoinCells_ReturnTopLeftCell_IsNotNull()
        {
            var sheetSettings = BuildSheetSettingsWithCellNames(10, 10);
            var sheet = new Sheet(sheetSettings);
            var selectedCells = sheet.Cells.Take(2).ToList();
            var result = sheet.JoinCells(selectedCells);

            Assert.IsNotNull(result);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void JoinCells_ReturnTopLeftCell_IsNull(int selectedCellsCount)
        {
            var sheetSettings = BuildSheetSettingsWithCellNames(10, 10);
            var sheet = new Sheet(sheetSettings);
            var selectedCells = sheet.Cells.Take(selectedCellsCount).ToList();
            var result = sheet.JoinCells(selectedCells);

            Assert.IsNull(result);
        }

        [TestCase(1, 5, 1, 5)]
        [TestCase(1, 1, 1, 5)]
        [TestCase(1, 5, 1, 1)]
        public void SplitCells_CellsHiddenByJoinEqualFalse(
            int firstColumnNumber,
            int lastColumnNumber,
            int firstRowNumber,
            int lastRowNumber)
        {
            var sheetSettings = BuildSheetSettingsWithCellNames(10, 10);
            var sheet = new Sheet(sheetSettings);
            var cellWithAddressList = new List<CellWithAddress>();

            sheet.Cells.ToList().ForEach(x => cellWithAddressList.Add(new CellWithAddress
            {
                Cell = x,
                Address = sheet.CellAddress(x)
            }));

            var selectedCells = cellWithAddressList
                .Where(x => x.Address.Column >= firstColumnNumber && x.Address.Column <= lastColumnNumber &&
                            x.Address.Row >= firstRowNumber && x.Address.Row <= lastRowNumber)
                .Select(s => s.Cell)
                .ToList();

            var topLeftCell = sheet.JoinCells(selectedCells);

            var result = selectedCells
                .Where(x => x.Uid != topLeftCell.Uid)
                .All(x => x.HiddenByJoin == false);

            Assert.IsFalse(result);
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
}
