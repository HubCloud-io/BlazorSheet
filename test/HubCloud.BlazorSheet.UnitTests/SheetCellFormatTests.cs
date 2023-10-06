using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Models;
using NUnit.Framework;

namespace HubCloud.BlazorSheet.UnitTests
{
    [TestFixture]
    public class SheetCellFormatTests
    {
        [TestCase(CellFormatTypes.None, CellFormatConsts.None)]
        [TestCase(CellFormatTypes.Integer, CellFormatConsts.Integer)]
        [TestCase(CellFormatTypes.IntegerTwoDecimalPlaces, CellFormatConsts.IntegerTwoDecimalPlaces)]
        [TestCase(CellFormatTypes.IntegerThreeDecimalPlaces, CellFormatConsts.IntegerThreeDecimalPlaces)]
        [TestCase(CellFormatTypes.Date, CellFormatConsts.Date)]
        [TestCase(CellFormatTypes.DateTime, CellFormatConsts.DateTime)]
        public void SetFormat_IsFormatEqualExpected_True(CellFormatTypes formatType, string expected)
        {
            var cell = new SheetCell();
            cell.SetFormat(formatType, string.Empty);

            Assert.AreEqual(expected, cell.Format);
        }

        [TestCase(CellFormatTypes.Custom, "C2", "C2")]
        [TestCase(CellFormatTypes.Custom, "qwerty123", "qwerty123")]
        public void SetFormat_Custom_IsFormatEqualExpected_True(CellFormatTypes formatType, string customFormat, string expected)
        {
            var cell = new SheetCell();
            cell.SetFormat(formatType, customFormat);

            Assert.AreEqual(expected, cell.Format);
        }
    }
}
