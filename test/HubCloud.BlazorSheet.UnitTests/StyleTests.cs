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
    public class StyleTests
    {
        [TestCase(CellFormatTypes.None, CellFormatConsts.None)]
        [TestCase(CellFormatTypes.Integer, CellFormatConsts.Integer)]
        [TestCase(CellFormatTypes.IntegerTwoDecimalPlaces, CellFormatConsts.IntegerTwoDecimalPlaces)]
        [TestCase(CellFormatTypes.IntegerThreeDecimalPlaces, CellFormatConsts.IntegerThreeDecimalPlaces)]
        [TestCase(CellFormatTypes.Date, CellFormatConsts.Date)]
        [TestCase(CellFormatTypes.DateTime, CellFormatConsts.DateTime)]
        public void SetStyle_IsFormatEqualExpected_True(CellFormatTypes formatType, string expected)
        {
            var styleModel = new SheetCommandPanelModel();
            styleModel.FormatType = formatType;

            var sheetCellStyle = new SheetCellStyle(styleModel);

            Assert.AreEqual(expected, sheetCellStyle.Format);
        }

        [TestCase(CellFormatTypes.Custom, "C2", "C2")]
        [TestCase(CellFormatTypes.Custom, "qwerty123", "qwerty123")]
        public void SetStyle_Custom_IsFormatEqualExpected_True(CellFormatTypes formatType, string customFormat, string expected)
        {
            var styleModel = new SheetCommandPanelModel();
            styleModel.FormatType = formatType;
            styleModel.CustomFormat = customFormat;

            var sheetCellStyle = new SheetCellStyle(styleModel);

            Assert.AreEqual(expected, sheetCellStyle.Format);
        }

        [TestCase("", CellFormatTypes.None)]
        [TestCase(null, CellFormatTypes.None)]
        [TestCase(CellFormatConsts.Integer, CellFormatTypes.Integer)]
        [TestCase(CellFormatConsts.IntegerTwoDecimalPlaces, CellFormatTypes.IntegerTwoDecimalPlaces)]
        [TestCase(CellFormatConsts.IntegerThreeDecimalPlaces, CellFormatTypes.IntegerThreeDecimalPlaces)]
        [TestCase(CellFormatConsts.Date, CellFormatTypes.Date)]
        [TestCase(CellFormatConsts.DateTime, CellFormatTypes.DateTime)]
        [TestCase("C2", CellFormatTypes.Custom)]
        public void CopyFrom_AreFormatTypeEqualExpected_True(string format, CellFormatTypes expected)
        {
            var sheetCellStyle = new SheetCellStyle();
            sheetCellStyle.Format = format;

            var styleModel = new SheetCommandPanelModel();
            styleModel.CopyFrom(sheetCellStyle);

            Assert.AreEqual(expected, styleModel.FormatType);
        }
    }
}
