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
    public class SheetCellApplyFormatTests
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
        public void ApplyFormat_CellFormatConsts_IsTextCorrect_True(string input, string expected, string format)
        {
            var cell = new SheetCell();
            cell.Value = input;
            cell.Format = format;
            cell.ApplyFormat();

            Assert.AreEqual(expected, cell.Text);
        }

        [TestCase("Склад", 1, 26, "", "Склад")]
        [TestCase("100", 100, 0, CellFormatConsts.IntegerTwoDecimalPlaces, "100.00")]
        [TestCase("2023-05-30T13:24:40", "2023-05-30T13:24:40", 0, CellFormatConsts.Date, "30.05.2023")]
        [TestCase("", "Text", 0, "", "Text")]
        [TestCase("", 12.66, 0, "", "12.66")]
        public void ApplyFormat_CellDataType_IsCellTextEqualExpected_True(string text, object value, int cellDataType, string format, string expected)
        {
            var cell = new SheetCell();
            cell.Text = text;
            cell.Value = value;
            cell.Format = format;
            cell.ApplyFormat(cellDataType);

            Assert.AreEqual(expected, cell.Text);
        }

        [TestCase("", "25.10.2023", "yyyy MMMM", "2023 октябрь")]
        [TestCase("2023 октябрь", "25.10.2023", "yyyy MMMM", "2023 октябрь")]
        [TestCase("2023 октябрь", "2023 октябрь", "", "2023 октябрь")]
        public void ApplyFormat_CustomFormat_IsCellTextEqualExpected_True(string text, object value, string customFormat, string expected)
        {
            var cell = new SheetCell();
            cell.Text = text;
            cell.Value = value;
            cell.Format = customFormat;
            cell.ApplyFormat();

            Assert.AreEqual(expected, cell.Text);
        }
    }
}
