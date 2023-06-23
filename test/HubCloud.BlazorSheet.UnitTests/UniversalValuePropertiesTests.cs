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
    public class UniversalValuePropertiesTests
    {
        [TestCase(null, "0001-01-01T00:00:00")]
        [TestCase("", "0001-01-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", "2023-06-21T14:51:11")]
        public void Get_Property_Day_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.Day;

            Assert.AreEqual(dateTimeCheck.Day, result.Value);
        }

        [TestCase(null, "0001-01-01T00:00:00")]
        [TestCase("", "0001-01-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", "2023-06-21T14:51:11")]
        public void Get_Property_Hour_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.Hour;

            Assert.AreEqual(dateTimeCheck.Hour, result.Value);
        }

        [TestCase(null, "0001-01-01T00:00:00")]
        [TestCase("", "0001-01-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", "2023-06-21T14:51:11")]
        public void Get_Property_Month_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.Month;

            Assert.AreEqual(dateTimeCheck.Month, result.Value);
        }

        [TestCase(null, "0001-01-01T00:00:00")]
        [TestCase("", "0001-01-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", "2023-06-21T14:51:11")]
        public void Get_Property_Minute_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.Minute;

            Assert.AreEqual(dateTimeCheck.Minute, result.Value);
        }

        [TestCase(null, "0001-01-01T00:00:00")]
        [TestCase("", "0001-01-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", "2023-06-21T14:51:11")]
        public void Get_Property_Second_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.Second;

            Assert.AreEqual(dateTimeCheck.Second, result.Value);
        }

        [TestCase(null, "0001-01-01T00:00:00")]
        [TestCase("", "0001-01-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", "2023-06-21T14:51:11")]
        public void Get_Property_Year_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.Year;

            Assert.AreEqual(dateTimeCheck.Year, result.Value);
        }
    }
}
