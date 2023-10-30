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
        [TestCase(CellDisplayFormatConsts.Integer, CellFormatTypes.Integer)]
        [TestCase(CellDisplayFormatConsts.IntegerTwoDecimalPlaces, CellFormatTypes.IntegerTwoDecimalPlaces)]
        [TestCase(CellDisplayFormatConsts.IntegerThreeDecimalPlaces, CellFormatTypes.IntegerThreeDecimalPlaces)]
        [TestCase(CellDisplayFormatConsts.Date, CellFormatTypes.Date)]
        [TestCase(CellDisplayFormatConsts.DateTime, CellFormatTypes.DateTime)]
        [TestCase("C2", CellFormatTypes.Custom)]
        public void SetFromatType_AreFormatTypeEqualExpected_True(string format, CellFormatTypes expected)
        {
            var styleModel = new SheetCommandPanelModel();
            styleModel.SetFromatType(format);

            Assert.AreEqual(expected, styleModel.FormatType);
        }
    }
}
