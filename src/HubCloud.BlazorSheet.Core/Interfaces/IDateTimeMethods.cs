using HubCloud.BlazorSheet.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Interfaces
{
    public interface IDateTimeMethods
    {
        UniversalValue AddDays(decimal value);
        UniversalValue AddHours(decimal value);
        UniversalValue AddMinutes(decimal value);
        UniversalValue AddMonths(int months);
        UniversalValue AddSeconds(decimal value);
        UniversalValue AddYears(int value);
        UniversalValue AddQuarters(int quarter);
        UniversalValue SetSecond(int second);
        UniversalValue SetMinute(int minute);
        UniversalValue SetHour(int hour);
        UniversalValue SetDay(int day);
        UniversalValue SetQuarter(int quarter);
        UniversalValue SetMonth(int month);
        UniversalValue SetYear(int year);
        UniversalValue EndYear();
        UniversalValue BeginYear();
        UniversalValue BeginDay();
        UniversalValue EndDay();
        UniversalValue BeginMonth();
        UniversalValue EndMonth();
        UniversalValue BeginQuarter();
        UniversalValue EndQuarter();
    }
}
