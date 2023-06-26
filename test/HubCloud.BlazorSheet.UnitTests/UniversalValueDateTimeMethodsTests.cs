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
    public class UniversalValueDateTimeMethodsTests
    {
        [TestCase(null, 1, "0001-01-02T00:00:00")]
        [TestCase("", 1, "0001-01-02T00:00:00")]
        [TestCase("2023-06-21T14:51:11", 1, "2023-06-22T14:51:11")]
        public void AddDays_Days_UniversalValue(string dateStr, int days, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.AddDays(days);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 1, "0001-01-01T01:00:00")]
        [TestCase("", 1, "0001-01-01T01:00:00")]
        [TestCase("2023-06-21T14:51:11", 1, "2023-06-21T15:51:11")]
        public void AddHours_Hours_UniversalValue(string dateStr, int hours, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.AddHours(hours);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 1, "0001-01-01T00:01:00")]
        [TestCase("", 1, "0001-01-01T00:01:00")]
        [TestCase("2023-06-21T14:51:11", 1, "2023-06-21T14:52:11")]
        public void AddMinutes_Minutes_UniversalValue(string dateStr, int minutes, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.AddMinutes(minutes);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 1, "0001-02-01T00:00:00")]
        [TestCase("", 1, "0001-02-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", 1, "2023-07-21T14:51:11")]
        public void AddMonths_Months_UniversalValue(string dateStr, int months, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.AddMonths(months);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 1, "0001-01-01T00:00:01")]
        [TestCase("", 1, "0001-01-01T00:00:01")]
        [TestCase("2023-06-21T14:51:11", 1, "2023-06-21T14:51:12")]
        public void AddSeconds_Seconds_UniversalValue(string dateStr, int seconds, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.AddSeconds(seconds);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 1, "0002-01-01T00:00:00")]
        [TestCase("", 1, "0002-01-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", 1, "2024-06-21T14:51:11")]
        public void AddYears_Years_UniversalValue(string dateStr, int years, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.AddYears(years);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 1, "0001-04-01T00:00:00")]
        [TestCase("", 1, "0001-04-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", 1, "2023-09-21T14:51:11")]
        public void AddQuarters_Quarters_UniversalValue(string dateStr, int quorters, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.AddQuarters(quorters);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 4, "0001-01-01T00:00:04")]
        [TestCase("", 4, "0001-01-01T00:00:04")]
        [TestCase("2023-06-21T14:51:11", 4, "2023-06-21T14:51:04")]
        public void SetSecond_Seconds_UniversalValue(string dateStr, int second, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.SetSecond(second);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 4, "0001-01-01T00:04:00")]
        [TestCase("", 4, "0001-01-01T00:04:00")]
        [TestCase("2023-06-21T14:51:11", 4, "2023-06-21T14:04:11")]
        public void SetMinute_Minutes_UniversalValue(string dateStr, int minute, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.SetMinute(minute);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 4, "0001-01-01T04:00:00")]
        [TestCase("", 4, "0001-01-01T04:00:00")]
        [TestCase("2023-06-21T14:51:11", 4, "2023-06-21T04:51:11")]
        public void SetHour_Hour_UniversalValue(string dateStr, int hour, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.SetHour(hour);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 4, "0001-01-04T00:00:00")]
        [TestCase("", 4, "0001-01-04T00:00:00")]
        [TestCase("2023-06-21T14:51:11", 1, "2023-06-01T14:51:11")]
        public void SetDay_Day_UniversalValue(string dateStr, int day, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.SetDay(day);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 1, "0001-04-01T00:00:00")]
        [TestCase("", 1, "0001-04-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", 1, "2023-09-21T14:51:11")]
        public void SetQuarter_Quarter_UniversalValue(string dateStr, int quarter, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.AddQuarters(quarter);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 2, "0001-02-01T00:00:00")]
        [TestCase("", 2, "0001-02-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", 1, "2023-01-21T14:51:11")]
        public void SetMonth_Month_UniversalValue(string dateStr, int month, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.SetMonth(month);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, 2, "0002-01-01T00:00:00")]
        [TestCase("", 2, "0002-01-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", 2, "0002-06-21T14:51:11")]
        public void SetYear_Year_UniversalValue(string dateStr, int year, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.SetYear(year);

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, "0001-12-31T23:59:59")]
        [TestCase("", "0001-12-31T23:59:59")]
        [TestCase("2023-06-21T14:51:11", "2023-12-31T23:59:59")]
        public void EndYear_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.EndYear();

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, "0001-01-01T00:00:00")]
        [TestCase("", "0001-01-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", "2023-01-01T00:00:00")]
        public void BeginYear_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.BeginYear();

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, "0001-01-01T00:00:00")]
        [TestCase("", "0001-01-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", "2023-06-21T00:00:00")]
        public void BeginDay_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.BeginDay();

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, "0001-01-01T23:59:59")]
        [TestCase("", "0001-01-01T23:59:59")]
        [TestCase("2023-06-21T14:51:11", "2023-06-21T23:59:59")]
        public void EndDay_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.EndDay();

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, "0001-01-01T00:00:00")]
        [TestCase("", "0001-01-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", "2023-06-01T00:00:00")]
        public void BeginMonth_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.BeginMonth();

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, "0001-01-31T23:59:59")]
        [TestCase("", "0001-01-31T23:59:59")]
        [TestCase("2023-06-21T14:51:11", "2023-06-30T23:59:59")]
        public void EndMonth_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.EndMonth();

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, "0001-01-01T00:00:00")]
        [TestCase("", "0001-01-01T00:00:00")]
        [TestCase("2023-06-21T14:51:11", "2023-04-01T00:00:00")]
        public void BeginQuarter_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.BeginQuarter();

            Assert.AreEqual(dateTimeCheck, result.Value);
        }

        [TestCase(null, "0001-03-31T23:59:59")]
        [TestCase("", "0001-03-31T23:59:59")]
        [TestCase("2023-06-21T14:51:11", "2023-06-30T23:59:59")]
        public void EndQuarter_UniversalValue(string dateStr, string checkStr)
        {
            DateTime.TryParse(checkStr, out var dateTimeCheck);

            var uv = new UniversalValue(dateStr);
            var result = uv.EndQuarter();

            Assert.AreEqual(dateTimeCheck, result.Value);
        }
    }
}
