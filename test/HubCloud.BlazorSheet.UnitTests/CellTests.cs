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
        [TestCase("123", null, CellFormatConsts.None)]
        [TestCase("123,44", "123", CellFormatConsts.Integer)]
        [TestCase("123.44", "123", CellFormatConsts.Integer)]
        [TestCase("123", "123.00", CellFormatConsts.IntegerTwoDecimalPlaces)]
        [TestCase("123", "123.000", CellFormatConsts.IntegerThreeDecimalPlaces)]
        [TestCase("2023-05-30T13:24:40", "30.05.2023", CellFormatConsts.Date)]
        [TestCase("2023-05-30T13:24:40", "30.05.2023 13:24:40", CellFormatConsts.DateTime)]
        [TestCase("123", "qwert123", "qwert123")]
        public void ApplyFormat_AreCellTextEqualExpected_True(string input, string expected, string format)
        {
            var cell = new SheetCell();
            cell.Value = input;
            cell.Format = format;
            cell.ApplyFormat();

            Assert.AreEqual(expected, cell.Text);
        }
    }
}
