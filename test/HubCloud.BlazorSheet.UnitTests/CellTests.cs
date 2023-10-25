using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Enums;
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
    public class CellTests
    {
        [TestCase("123", "123", CellFormatConsts.None)]
        [TestCase("123,44", "123", CellFormatConsts.Integer)]
        [TestCase("123.44", "123", CellFormatConsts.Integer)]
        [TestCase("123", "123.00", CellFormatConsts.IntegerTwoDecimalPlaces)]
        [TestCase("123", "123.000", CellFormatConsts.IntegerThreeDecimalPlaces)]
        [TestCase("-123", "-123", CellFormatConsts.None)]
        [TestCase("-123,44", "-123", CellFormatConsts.Integer)]
        [TestCase("-123.44", "-123", CellFormatConsts.Integer)]
        [TestCase("-123", "-123.00", CellFormatConsts.IntegerTwoDecimalPlaces)]
        [TestCase("-123", "-123.000", CellFormatConsts.IntegerThreeDecimalPlaces)]
        [TestCase("2023-05-30T13:24:40", "30.05.2023", CellFormatConsts.Date)]
        [TestCase("2023-05-30T13:24:40", "30.05.2023 13:24:40", CellFormatConsts.DateTime)]
        [TestCase("123", "qwert123", "qwert123")]
        [TestCase("-123", "-qwert123", "qwert123")]
        [TestCase("-1000000,3388", "-1 000 000", CellFormatConsts.IntegerWithSpaces)]
        [TestCase("-1000000,3388", "-1 000 000.34", CellFormatConsts.IntegerWithSpacesTwoDecimalPlaces)]
        [TestCase("-1000000,3388", "-1 000 000.339", CellFormatConsts.IntegerWithSpacesThreeDecimalPlaces)]
        [TestCase("-1000000,3388", "(1 000 000)", CellFormatConsts.IntegerNegativeWithSpaces)]
        [TestCase("-1000000,3388", "(1 000 000.34)", CellFormatConsts.IntegerNegativeWithSpacesTwoDecimalPlaces)]
        [TestCase("-1000000,3388", "(1 000 000.339)", CellFormatConsts.IntegerNegativeWithSpacesThreeDecimalPlaces)]
        [TestCase("1000000,3388", "1 000 000", CellFormatConsts.IntegerNegativeWithSpaces)]
        [TestCase("1000000,3388", "1 000 000.34", CellFormatConsts.IntegerNegativeWithSpacesTwoDecimalPlaces)]
        [TestCase("1000000,3388", "1 000 000.339", CellFormatConsts.IntegerNegativeWithSpacesThreeDecimalPlaces)]
        public void ApplyFormat_IsCellTextEqualExpected_True(string input, string expected, string format)
        {
            var cell = new SheetCell();
            cell.Value = input;
            cell.Format = format;
            cell.ApplyFormat();

            Assert.AreEqual(expected, cell.Text);
        }

        [Test]
        public void ApplyFormat_CellDataType_IsCellTextEqualExpected_True()
        {
            var sheet = BuildSheet();

            var cellNumber = sheet.GetCell(1, 1);
            var cellDate = sheet.GetCell(1, 2);
            var cellSelect = sheet.GetCell(1, 3);
            var cellText = sheet.GetCell(1, 4);
            var cellWithoutEditSettings = sheet.GetCell(1, 5);

            cellNumber.Value = 100;
            cellDate.Value = "2023-05-30T13:24:40";
            cellSelect.Value = 1;
            cellText.Value = "text";
            cellWithoutEditSettings.Value = "text";

            cellSelect.Text = "Склад 1";

            sheet.PrepareCellText();

            Assert.AreEqual("100.00", cellNumber.Text);
            Assert.AreEqual("30.05.2023", cellDate.Text);
            Assert.AreEqual("Склад 1", cellSelect.Text);
            Assert.AreEqual("text", cellText.Text);
            Assert.AreEqual("text", cellWithoutEditSettings.Text);
        }

        private Sheet BuildSheet()
        {
            var sheetSettings = new SheetSettings();
            sheetSettings.RowsCount = 10;
            sheetSettings.ColumnsCount = 10;

            var editSettingsNumber = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.NumberInput,
                NumberDigits = 2
            };
            sheetSettings.EditSettings.Add(editSettingsNumber);

            var editSettingsDate = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.DateInput
            };
            sheetSettings.EditSettings.Add(editSettingsDate);

            var editSettingsSelect = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.Select,
                CellDataType = 26
            };
            sheetSettings.EditSettings.Add(editSettingsSelect);

            var editSettingsText = new SheetCellEditSettings()
            {
                ControlKind = CellControlKinds.TextInput
            };
            sheetSettings.EditSettings.Add(editSettingsText);

            var sheet = new Sheet(sheetSettings);

            var cellNumber = sheet.GetCell(1, 1);
            var cellDate = sheet.GetCell(1, 2);
            var cellSelect = sheet.GetCell(1, 3);
            var cellText = sheet.GetCell(1, 4);

            cellNumber.EditSettingsUid = editSettingsNumber.Uid;
            cellDate.EditSettingsUid = editSettingsDate.Uid;
            cellSelect.EditSettingsUid = editSettingsSelect.Uid;
            cellText.EditSettingsUid = editSettingsText.Uid;

            cellNumber.Format = CellFormatConsts.IntegerTwoDecimalPlaces;
            cellDate.Format = CellFormatConsts.Date;

            return sheet;
        }
    }
}
