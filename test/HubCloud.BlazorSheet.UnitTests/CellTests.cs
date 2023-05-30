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
    public class CellTests
    {
        [TestCase("123", null, CellFormatConsts.None)]
        [TestCase("123,44", "123", CellFormatConsts.F0)]
        [TestCase("123", "123,00", CellFormatConsts.F)]
        [TestCase("123", "123,000", CellFormatConsts.F3)]
        [TestCase("Вт 30.05.23 13:24:40", "30.05.2023", CellFormatConsts.Date)]
        [TestCase("Вт 30.05.23 13:24:40", "30.05.2023 13:24:40", CellFormatConsts.DateTime)]
        [TestCase("Вт 30.05.23 13:24:40", "30 мая", "d MMMM")]
        [TestCase("123,44", "123,44 ₽", "C2")]
        [TestCase("123", "qwert123", "qwert123")]
        public void ApplyFormat_AreCellTextEqualExpected_True(string input, string expected, string format)
        {
            var cell = new SheetCell();
            cell.Value = input;
            cell.ApplyFormat(format);

            Assert.AreEqual(expected, cell.Text);
        }
    }
}
