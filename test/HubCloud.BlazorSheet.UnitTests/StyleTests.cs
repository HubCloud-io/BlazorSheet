﻿using HubCloud.BlazorSheet.Core.Consts;
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
        [TestCase(CellFormatTypes.F0, CellFormatConsts.F0)]
        [TestCase(CellFormatTypes.F, CellFormatConsts.F)]
        [TestCase(CellFormatTypes.F3, CellFormatConsts.F3)]
        [TestCase(CellFormatTypes.Date, CellFormatConsts.Date)]
        [TestCase(CellFormatTypes.DateTime, CellFormatConsts.DateTime)]
        public void SetStyle_IsFormatEqualExpected_True(CellFormatTypes formatType, string expected)
        {
            var styleModel = new SheetCommandPanelStyleModel();
            styleModel.FormatType = formatType;

            var sheetCellStyle = new SheetCellStyle(styleModel);

            Assert.AreEqual(expected, sheetCellStyle.Format);
        }

        [TestCase(CellFormatTypes.Custom, "C2", "C2")]
        [TestCase(CellFormatTypes.Custom, "qwerty123", "qwerty123")]
        public void SetStyle_Custom_IsFormatEqualExpected_True(CellFormatTypes formatType, string customFormat, string expected)
        {
            var styleModel = new SheetCommandPanelStyleModel();
            styleModel.FormatType = formatType;
            styleModel.CustomFormat = customFormat;

            var sheetCellStyle = new SheetCellStyle(styleModel);

            Assert.AreEqual(expected, sheetCellStyle.Format);
        }

        [TestCase("", CellFormatTypes.None)]
        [TestCase(null, CellFormatTypes.None)]
        [TestCase(CellFormatConsts.F0, CellFormatTypes.F0)]
        [TestCase(CellFormatConsts.F, CellFormatTypes.F)]
        [TestCase(CellFormatConsts.F3, CellFormatTypes.F3)]
        [TestCase(CellFormatConsts.Date, CellFormatTypes.Date)]
        [TestCase(CellFormatConsts.DateTime, CellFormatTypes.DateTime)]
        [TestCase("C2", CellFormatTypes.Custom)]
        public void CopyFrom_AreFormatTypeEqualExpected_True(string format, CellFormatTypes expected)
        {
            var sheetCellStyle = new SheetCellStyle();
            sheetCellStyle.Format = format;

            var styleModel = new SheetCommandPanelStyleModel();
            styleModel.CopyFrom(sheetCellStyle);

            Assert.AreEqual(expected, styleModel.FormatType);
        }
    }
}
