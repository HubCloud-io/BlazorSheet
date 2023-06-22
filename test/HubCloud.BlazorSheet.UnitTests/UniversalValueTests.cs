using HubCloud.BlazorSheet.Core.Models;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using ExpressoFunctions.FunctionLibrary;

namespace HubCloud.BlazorSheet.UnitTests;

[TestFixture]
public class UniversalValueTests
{
    
    [Test]
    public void Plus_DecimalAndDecimal_SumValue()
    {
        var uValue1 = new UniversalValue(1M);
        var uValue2 = new UniversalValue(2M);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(3M, result.Value);
    }
    
    [Test]
    public void Plus_DecimalAndNull_SumValue()
    {
        var uValue1 = new UniversalValue(1M);
        var uValue2 = new UniversalValue(null);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(1M, result.Value);
    }
    
    [Test]
    public void Plus_NullAndDecimal_SumValue()
    {
        var uValue1 = new UniversalValue(null);
        var uValue2 = new UniversalValue(2M);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(2M, result.Value);
    }
    
    [Test]
    public void Plus_DecimalAndInt_SumValue()
    {
        var uValue1 = new UniversalValue(1M);
        var uValue2 = new UniversalValue(1);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(2M, result.Value);
    }
    
    [Test]
    public void Plus_IntAndDecimal_SumValue()
    {
        var uValue1 = new UniversalValue(1);
        var uValue2 = new UniversalValue(1M);

        var result = uValue1 + uValue2;
        
        Assert.AreEqual(2M, result.Value);
    }

    [TestCase("qwerty", 1, "werty")]
    public void Substring_StartIndex_SubstringValue(string value, int startIndex, string expectedValue)
    {
        var uv = new UniversalValue(value);

        var result = uv.Substring(startIndex);

        Assert.AreEqual(expectedValue, result.Value);
    }

    [TestCase("qwerty", 0, 1, "q")]
    public void Substring_StartIndexLength_SubstringValue(string value, int startIndex, int length, string expectedValue)
    {
        var uv = new UniversalValue(value);

        var result = uv.Substring(startIndex, length);

        Assert.AreEqual(expectedValue, result.Value);
    }

    [TestCase("qwerty", "QWERTY")]
    public void ToUpper_ToUpperValue(string value, string expectedValue)
    {
        var uv = new UniversalValue(value);

        var result = uv.ToUpper();

        Assert.AreEqual(expectedValue, result.Value);
    }

    [TestCase("QWERTY", "qwerty")]
    public void ToLower_ToLowerValue(string value, string expectedValue)
    {
        var uv = new UniversalValue(value);

        var result = uv.ToLower();

        Assert.AreEqual(expectedValue, result.Value);
    }

    [TestCase("QWERTY", "W", 1)]
    public void IndexOf_StringValue_IndexOfValue(string value, string indexOfValue, int expectedIndex)
    {
        var uv = new UniversalValue(value);

        var result = uv.IndexOf(indexOfValue);

        Assert.AreEqual(expectedIndex, result.Value);
    }

    [TestCase("QWERTY", "WERT", "____", "Q____Y")]
    public void Replace_OldValueNewValue_ReplaceValue(string value, string oldValue, string newValue, string expectedValue)
    {
        var uv = new UniversalValue(value);

        var result = uv.Replace(oldValue, newValue);

        Assert.AreEqual(expectedValue, result.Value);
    }

    [TestCase(21, 1, 22)]
    public void AddDays_Days_UniversalValue(int days, int addedDays, int expectedDays)
    {
        var dateTime = new DateTime(2023, 6, days, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.AddDays(addedDays);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.AddDays(addedDays);

        uv = new UniversalValue("");
        var result_3 = uv.AddDays(addedDays);

        uv = new UniversalValue(123);
        var result_4 = uv.AddDays(addedDays);

        uv = new UniversalValue(null);
        var result_5 = uv.AddDays(addedDays);

        var expected = new DateTime(2023, 6, expectedDays, 15, 14, 13);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(15, 1, 16)]
    public void AddHours_Hours_UniversalValue(int hours, int addedHours, int expectedHours)
    {
        var dateTime = new DateTime(2023, 6, 20, hours, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.AddHours(addedHours);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.AddHours(addedHours);

        uv = new UniversalValue("");
        var result_3 = uv.AddHours(addedHours);

        uv = new UniversalValue(123);
        var result_4 = uv.AddHours(addedHours);

        uv = new UniversalValue(null);
        var result_5 = uv.AddHours(addedHours);

        var expected = new DateTime(2023, 6, 20, expectedHours, 14, 13);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(14, 10, 24)]
    public void AddMinutes_Minutes_UniversalValue(int minutes, int addedMinutes, int expectedMinutes)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, minutes, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.AddMinutes(addedMinutes);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.AddMinutes(addedMinutes);

        uv = new UniversalValue("");
        var result_3 = uv.AddMinutes(addedMinutes);

        uv = new UniversalValue(123);
        var result_4 = uv.AddMinutes(addedMinutes);

        uv = new UniversalValue(null);
        var result_5 = uv.AddMinutes(addedMinutes);

        var expected = new DateTime(2023, 6, 20, 15, expectedMinutes, 13);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(6, 1, 7)]
    public void AddMonths_Months_UniversalValue(int months, int addedMonths, int expectedMonths)
    {
        var dateTime = new DateTime(2023, months, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.AddMonths(addedMonths);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.AddMonths(addedMonths);

        uv = new UniversalValue("");
        var result_3 = uv.AddMonths(addedMonths);

        uv = new UniversalValue(123);
        var result_4 = uv.AddMonths(addedMonths);

        uv = new UniversalValue(null);
        var result_5 = uv.AddMonths(addedMonths);

        var expected = new DateTime(2023, expectedMonths, 20, 15, 14, 13);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(13, 10, 23)]
    public void AddSeconds_Seconds_UniversalValue(int seconds, int addedSeconds, int expectedSeconds)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, 14, seconds);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.AddSeconds(addedSeconds);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.AddSeconds(addedSeconds);

        uv = new UniversalValue("");
        var result_3 = uv.AddSeconds(addedSeconds);

        uv = new UniversalValue(123);
        var result_4 = uv.AddSeconds(addedSeconds);

        uv = new UniversalValue(null);
        var result_5 = uv.AddSeconds(addedSeconds);

        var expected = new DateTime(2023, 6, 20, 15, 14, expectedSeconds);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(2023, 2, 2025)]
    public void AddYears_Years_UniversalValue(int years, int addedYears, int expectedYears)
    {
        var dateTime = new DateTime(years, 6, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.AddYears(addedYears);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.AddYears(addedYears);

        uv = new UniversalValue("");
        var result_3 = uv.AddYears(addedYears);

        uv = new UniversalValue(123);
        var result_4 = uv.AddYears(addedYears);

        uv = new UniversalValue(null);
        var result_5 = uv.AddYears(addedYears);

        var expected = new DateTime(expectedYears, 6, 20, 15, 14, 13);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(1, 6, 9)]
    public void AddQuarters_Quarters_UniversalValue(int quarter, int months, int expectedMonths)
    {
        var dateTime = new DateTime(2023, months, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.AddQuarters(quarter);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.AddQuarters(quarter);

        uv = new UniversalValue("");
        var result_3 = uv.AddQuarters(quarter);

        uv = new UniversalValue(123);
        var result_4 = uv.AddQuarters(quarter);

        uv = new UniversalValue(null);
        var result_5 = uv.AddQuarters(quarter);

        var expected = new DateTime(2023, expectedMonths, 20, 15, 14, 13);
        var minValuePlusQuarter = DateTime.MinValue.AddMonths(3);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(minValuePlusQuarter, result_3.Value);
        Assert.AreEqual(minValuePlusQuarter, result_4.Value);
        Assert.AreEqual(minValuePlusQuarter, result_5.Value);
    }

    [TestCase(44, 10, 10)]
    public void SetSecond_Seconds_UniversalValue(int seconds, int setSeconds, int expectedSeconds)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, 14, seconds);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.SetSecond(setSeconds);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.SetSecond(setSeconds);

        uv = new UniversalValue("");
        var result_3 = uv.SetSecond(setSeconds);

        uv = new UniversalValue(123);
        var result_4 = uv.SetSecond(setSeconds);

        uv = new UniversalValue(null);
        var result_5 = uv.SetSecond(setSeconds);

        var expected = new DateTime(2023, 6, 20, 15, 14, expectedSeconds);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(14, 10, 10)]
    public void SetMinute_Minutes_UniversalValue(int minutes, int setMinutes, int expectedMinutes)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, minutes, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.SetMinute(setMinutes);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.SetMinute(setMinutes);

        uv = new UniversalValue("");
        var result_3 = uv.SetMinute(setMinutes);

        uv = new UniversalValue(123);
        var result_4 = uv.SetMinute(setMinutes);

        uv = new UniversalValue(null);
        var result_5 = uv.SetMinute(setMinutes);

        var expected = new DateTime(2023, 6, 20, 15, expectedMinutes, 13);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(15, 10, 10)]
    public void SetHour_Hours_UniversalValue(int hours, int setHours, int expectedHours)
    {
        var dateTime = new DateTime(2023, 6, 20, hours, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.SetHour(setHours);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.SetHour(setHours);

        uv = new UniversalValue("");
        var result_3 = uv.SetHour(setHours);

        uv = new UniversalValue(123);
        var result_4 = uv.SetHour(setHours);

        uv = new UniversalValue(null);
        var result_5 = uv.SetHour(setHours);

        var expected = new DateTime(2023, 6, 20, expectedHours, 14, 13);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(20, 10, 10)]
    public void SetDay_Days_UniversalValue(int days, int setDays, int expectedDays)
    {
        var dateTime = new DateTime(2023, 6, days, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.SetDay(setDays);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.SetDay(setDays);

        uv = new UniversalValue("");
        var result_3 = uv.SetDay(setDays);

        uv = new UniversalValue(123);
        var result_4 = uv.SetDay(setDays);

        uv = new UniversalValue(null);
        var result_5 = uv.SetDay(setDays);

        var expected = new DateTime(2023, 6, expectedDays, 15, 14, 13);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(1, 6, 9)]
    public void SetQuarter_Quarters_UniversalValue(int quarter, int months, int expectedMonths)
    {
        var dateTime = new DateTime(2023, months, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.AddQuarters(quarter);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.AddQuarters(quarter);

        uv = new UniversalValue("");
        var result_3 = uv.AddQuarters(quarter);

        uv = new UniversalValue(123);
        var result_4 = uv.AddQuarters(quarter);

        uv = new UniversalValue(null);
        var result_5 = uv.AddQuarters(quarter);

        var expected = new DateTime(2023, expectedMonths, 20, 15, 14, 13);
        var minValPlusQuarter = DateTime.MinValue.AddMonths(3);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(minValPlusQuarter, result_3.Value);
        Assert.AreEqual(minValPlusQuarter, result_4.Value);
        Assert.AreEqual(minValPlusQuarter, result_5.Value);
    }

   [TestCase(null, 1, "0001-04-01T00:00:00")]
   [TestCase("", 1, "0001-04-01T00:00:00")]
    [TestCase("2023-06-21T14:51:11", 1, "2023-09-21T14:51:11")]
    public void SetQuarter_Quarters1_UniversalValue(string dateStr, int quarter, string checkStr)
    {
        DateTime.TryParse(checkStr, out var dateTimeCheck);

        UniversalValue result;
        if (string.IsNullOrWhiteSpace(dateStr))
        {
            var uv = new UniversalValue(dateStr);
            
            result = uv.AddQuarters(quarter);
        }
        else
        {
            DateTime.TryParse(dateStr, out var dateTimeStart);
            var uv = new UniversalValue(dateTimeStart);
            
            result = uv.AddQuarters(quarter);
        }

        Assert.AreEqual(result.Value, dateTimeCheck);
        
    }

    [TestCase(6, 10, 10)]
    public void SetMonth_Months_UniversalValue(int months, int setMonths, int expectedMonths)
    {
        var dateTime = new DateTime(2023, months, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.SetMonth(setMonths);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.SetMonth(setMonths);

        uv = new UniversalValue("");
        var result_3 = uv.SetMonth(setMonths);

        uv = new UniversalValue(123);
        var result_4 = uv.SetMonth(setMonths);

        uv = new UniversalValue(null);
        var result_5 = uv.SetMonth(setMonths);

        var expected = new DateTime(2023, expectedMonths, 20, 15, 14, 13);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(2023, 2033, 2033)]
    public void SetYear_Years_UniversalValue(int years, int setYears, int expectedYears)
    {
        var dateTime = new DateTime(years, 6, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.SetYear(setYears);

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.SetYear(setYears);

        uv = new UniversalValue("");
        var result_3 = uv.SetYear(setYears);

        uv = new UniversalValue(123);
        var result_4 = uv.SetYear(setYears);

        uv = new UniversalValue(null);
        var result_5 = uv.SetYear(setYears);

        var expected = new DateTime(expectedYears, 6, 20, 15, 14, 13);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(12, 31, 23, 59, 59)]
    public void EndYear_UniversalValue(int expectedMonths, int expectedDays, int expectedHours, int expectedMinutes, int expectedSeconds)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.EndYear();

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.EndYear();

        uv = new UniversalValue("");
        var result_3 = uv.EndYear();

        uv = new UniversalValue(123);
        var result_4 = uv.EndYear();

        uv = new UniversalValue(null);
        var result_5 = uv.EndYear();

        var expected = new DateTime(2023, expectedMonths, expectedDays, expectedHours, expectedMinutes, expectedSeconds);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(1, 1, 0, 0, 0)]
    public void BeginYear_UniversalValue(int expectedMonths, int expectedDays, int expectedHours, int expectedMinutes, int expectedSeconds)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.BeginYear();

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.BeginYear();

        uv = new UniversalValue("");
        var result_3 = uv.BeginYear();

        uv = new UniversalValue(123);
        var result_4 = uv.BeginYear();

        uv = new UniversalValue(null);
        var result_5 = uv.BeginYear();

        var expected = new DateTime(2023, expectedMonths, expectedDays, expectedHours, expectedMinutes, expectedSeconds);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(0, 0, 0)]
    public void BeginDay_UniversalValue(int expectedHours, int expectedMinutes, int expectedSeconds)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.BeginDay();

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.BeginDay();

        uv = new UniversalValue("");
        var result_3 = uv.BeginDay();

        uv = new UniversalValue(123);
        var result_4 = uv.BeginDay();

        uv = new UniversalValue(null);
        var result_5 = uv.BeginDay();

        var expected = new DateTime(2023, 6, 20, expectedHours, expectedMinutes, expectedSeconds);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(23, 59, 59)]
    public void EndDay_UniversalValue(int expectedHours, int expectedMinutes, int expectedSeconds)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.EndDay();

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.EndDay();

        uv = new UniversalValue("");
        var result_3 = uv.EndDay();

        uv = new UniversalValue(123);
        var result_4 = uv.EndDay();

        uv = new UniversalValue(null);
        var result_5 = uv.EndDay();

        var expected = new DateTime(2023, 6, 20, expectedHours, expectedMinutes, expectedSeconds);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(1, 0, 0, 0)]
    public void BeginMonth_UniversalValue(int expectedDays, int expectedHours, int expectedMinutes, int expectedSeconds)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.BeginMonth();

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.BeginMonth();

        uv = new UniversalValue("");
        var result_3 = uv.BeginMonth();

        uv = new UniversalValue(123);
        var result_4 = uv.BeginMonth();

        uv = new UniversalValue(null);
        var result_5 = uv.BeginMonth();

        var expected = new DateTime(2023, 6, expectedDays, expectedHours, expectedMinutes, expectedSeconds);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(30, 23, 59, 59)]
    public void EndMonth_UniversalValue(int expectedDays, int expectedHours, int expectedMinutes, int expectedSeconds)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.EndMonth();

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.EndMonth();

        uv = new UniversalValue("");
        var result_3 = uv.EndMonth();

        uv = new UniversalValue(123);
        var result_4 = uv.EndMonth();

        uv = new UniversalValue(null);
        var result_5 = uv.EndMonth();

        var expected = new DateTime(2023, 6, expectedDays, expectedHours, expectedMinutes, expectedSeconds);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(4, 1, 0, 0, 0)]
    public void BeginQuarter_UniversalValue(int expectedMonths, int expectedDays, int expectedHours, int expectedMinutes, int expectedSeconds)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.BeginQuarter();

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.BeginQuarter();

        uv = new UniversalValue("");
        var result_3 = uv.BeginQuarter();

        uv = new UniversalValue(123);
        var result_4 = uv.BeginQuarter();

        uv = new UniversalValue(null);
        var result_5 = uv.BeginQuarter();

        var expected = new DateTime(2023, expectedMonths, expectedDays, expectedHours, expectedMinutes, expectedSeconds);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(6, 30, 23, 59, 59)]
    public void EndQuarter_UniversalValue(int expectedMonths, int expectedDays, int expectedHours, int expectedMinutes, int expectedSeconds)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.EndQuarter();

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.EndQuarter();

        uv = new UniversalValue("");
        var result_3 = uv.EndQuarter();

        uv = new UniversalValue(123);
        var result_4 = uv.EndQuarter();

        uv = new UniversalValue(null);
        var result_5 = uv.EndQuarter();

        var expected = new DateTime(2023, expectedMonths, expectedDays, expectedHours, expectedMinutes, expectedSeconds);

        Assert.AreEqual(expected, result_1.Value);
        Assert.AreEqual(expected, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(21, 21)]
    public void Get_Property_Day_UniversalValue(int days, int expectedDays)
    {
        var dateTime = new DateTime(2023, 6, days, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.Day;

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.Day;

        uv = new UniversalValue("");
        var result_3 = uv.Day;

        uv = new UniversalValue(123);
        var result_4 = uv.Day;

        uv = new UniversalValue(null);
        var result_5 = uv.Day;

        Assert.AreEqual(expectedDays, result_1.Value);
        Assert.AreEqual(expectedDays, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(15, 15)]
    public void Get_Property_Hour_UniversalValue(int hours, int expectedHours)
    {
        var dateTime = new DateTime(2023, 6, 20, hours, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.Hour;

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.Hour;

        uv = new UniversalValue("");
        var result_3 = uv.Hour;

        uv = new UniversalValue(123);
        var result_4 = uv.Hour;

        uv = new UniversalValue(null);
        var result_5 = uv.Hour;

        Assert.AreEqual(expectedHours, result_1.Value);
        Assert.AreEqual(expectedHours, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(6, 6)]
    public void Get_Property_Month_UniversalValue(int months, int expectedMonths)
    {
        var dateTime = new DateTime(2023, months, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.Month;

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.Month;

        uv = new UniversalValue("");
        var result_3 = uv.Month;

        uv = new UniversalValue(123);
        var result_4 = uv.Month;

        uv = new UniversalValue(null);
        var result_5 = uv.Month;

        Assert.AreEqual(expectedMonths, result_1.Value);
        Assert.AreEqual(expectedMonths, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(14, 14)]
    public void Get_Property_Minute_UniversalValue(int minutes, int expectedMinutes)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, minutes, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.Minute;

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.Minute;

        uv = new UniversalValue("");
        var result_3 = uv.Minute;

        uv = new UniversalValue(123);
        var result_4 = uv.Minute;

        uv = new UniversalValue(null);
        var result_5 = uv.Minute;

        Assert.AreEqual(expectedMinutes, result_1.Value);
        Assert.AreEqual(expectedMinutes, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(13, 13)]
    public void Get_Property_Second_UniversalValue(int seconds, int expectedSeconds)
    {
        var dateTime = new DateTime(2023, 6, 20, 15, 14, seconds);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.Second;

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.Second;

        uv = new UniversalValue("");
        var result_3 = uv.Second;

        uv = new UniversalValue(123);
        var result_4 = uv.Second;

        uv = new UniversalValue(null);
        var result_5 = uv.Second;

        Assert.AreEqual(expectedSeconds, result_1.Value);
        Assert.AreEqual(expectedSeconds, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }

    [TestCase(2023, 2023)]
    public void Get_Property_Year_UniversalValue(int years, int expectedYears)
    {
        var dateTime = new DateTime(years, 6, 20, 15, 14, 13);

        var uv = new UniversalValue(dateTime);
        var result_1 = uv.Year;

        uv = new UniversalValue(dateTime.ToString());
        var result_2 = uv.Year;

        uv = new UniversalValue("");
        var result_3 = uv.Year;

        uv = new UniversalValue(123);
        var result_4 = uv.Year;

        uv = new UniversalValue(null);
        var result_5 = uv.Year;

        Assert.AreEqual(expectedYears, result_1.Value);
        Assert.AreEqual(expectedYears, result_2.Value);
        Assert.AreEqual(null, result_3.Value);
        Assert.AreEqual(null, result_4.Value);
        Assert.AreEqual(null, result_5.Value);
    }
}