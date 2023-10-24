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
    public class SheetCommandPanelModelFormatTypeTests
    {
        [TestCase("", CellFormatTypes.None)]
        [TestCase(null, CellFormatTypes.None)]
        [TestCase(CellFormatConsts.Integer, CellFormatTypes.Integer)]
        [TestCase(CellFormatConsts.IntegerTwoDecimalPlaces, CellFormatTypes.IntegerTwoDecimalPlaces)]
        [TestCase(CellFormatConsts.IntegerThreeDecimalPlaces, CellFormatTypes.IntegerThreeDecimalPlaces)]
        [TestCase(CellFormatConsts.Date, CellFormatTypes.Date)]
        [TestCase(CellFormatConsts.DateTime, CellFormatTypes.DateTime)]
        [TestCase("C2", CellFormatTypes.Custom)]
        public void SetFromatType_AreFormatTypeEqualExpected_True(string format, CellFormatTypes expected)
        {
            var styleModel = new SheetCommandPanelModel();
            styleModel.SetFromatType(format);

            Assert.AreEqual(expected, styleModel.FormatType);
        }
    }
}
