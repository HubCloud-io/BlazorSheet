using HubCloud.BlazorSheet.Core.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Numerics;
using ExpressoFunctions.FunctionLibrary;

namespace HubCloud.BlazorSheet.Core.Models
{
    public struct UniversalValue : IStringMethods, IDateTimeMethods, IDateTimeProperties
    {
        public object Value { get; set; }

        public UniversalValue Day
        {
            get
            {
                if (Value is DateTime dateTime)
                {
                    return new UniversalValue(dateTime.Day);
                }

                if (Value is string stringValue)
                {
                    if (DateTime.TryParse(stringValue, out dateTime))
                        return new UniversalValue(dateTime.Day);
                }

                return new UniversalValue();
            }
        }

        public UniversalValue Hour
        {
            get
            {
                if (Value is DateTime dateTime)
                {
                    return new UniversalValue(dateTime.Hour);
                }

                if (Value is string stringValue)
                {
                    if (DateTime.TryParse(stringValue, out dateTime))
                        return new UniversalValue(dateTime.Hour);
                }

                return new UniversalValue();
            }
        }

        public UniversalValue Month
        {
            get
            {
                if (Value is DateTime dateTime)
                {
                    return new UniversalValue(dateTime.Month);
                }

                if (Value is string stringValue)
                {
                    if (DateTime.TryParse(stringValue, out dateTime))
                        return new UniversalValue(dateTime.Month);
                }

                return new UniversalValue();
            }
        }

        public UniversalValue Minute
        {
            get
            {
                if (Value is DateTime dateTime)
                {
                    return new UniversalValue(dateTime.Minute);
                }

                if (Value is string stringValue)
                {
                    if (DateTime.TryParse(stringValue, out dateTime))
                        return new UniversalValue(dateTime.Minute);
                }

                return new UniversalValue();
            }
        }

        public UniversalValue Second
        {
            get
            {
                if (Value is DateTime dateTime)
                {
                    return new UniversalValue(dateTime.Second);
                }

                if (Value is string stringValue)
                {
                    if (DateTime.TryParse(stringValue, out dateTime))
                        return new UniversalValue(dateTime.Second);
                }

                return new UniversalValue();
            }
        }

        public UniversalValue Year
        {
            get
            {
                if (Value is DateTime dateTime)
                {
                    return new UniversalValue(dateTime.Year);
                }

                if (Value is string stringValue)
                {
                    if (DateTime.TryParse(stringValue, out dateTime))
                        return new UniversalValue(dateTime.Year);
                }

                return new UniversalValue();
            }
        }

        public UniversalValue(object value)
        {
            Value = value;
        }

        public static UniversalValue operator +(UniversalValue v1, UniversalValue v2)
        {
            return PerformOperation(v1, v2, (a, b) => a + b);
        }

        public static UniversalValue operator -(UniversalValue v1, UniversalValue v2)
        {
            return PerformOperation(v1, v2, (a, b) => a - b);
        }
        
        public static UniversalValue operator *(UniversalValue v1, UniversalValue v2)
        {
            return PerformOperation(v1, v2, (a, b) => a * b);
        }
        
        
        public static UniversalValue operator /(UniversalValue v1, UniversalValue v2)
        {
            return PerformOperation(v1, v2, (a, b) => a / b);
        }

        public UniversalValue Substring(int startIndex)
        {
            var stringValue = ToString();

            if (string.IsNullOrEmpty(stringValue))
                return new UniversalValue(string.Empty);

            return new UniversalValue(stringValue.Substring(startIndex));
        }

        public UniversalValue Substring(int startIndex, int length)
        {

            var stringValue = ToString();

            if (string.IsNullOrEmpty(stringValue))
                return new UniversalValue(string.Empty);

            return new UniversalValue(stringValue.Substring(startIndex, length));
        }

        public UniversalValue ToUpper()
        {
            var stringValue = ToString();

            if (string.IsNullOrEmpty(stringValue))
                return new UniversalValue(string.Empty);

            return new UniversalValue(stringValue.ToUpper());
        }

        public UniversalValue ToLower()
        {
            var stringValue = ToString();

            if (string.IsNullOrEmpty(stringValue))
                return new UniversalValue(string.Empty);

            return new UniversalValue(stringValue.ToLower());
        }

        public UniversalValue IndexOf(string value)
        {
            var stringValue = ToString();

            if (string.IsNullOrEmpty(stringValue))
                return new UniversalValue(string.Empty);

            return new UniversalValue(stringValue.IndexOf(value));
        }

        public UniversalValue Replace(string oldValue, string newValue)
        {
            var stringValue = ToString();

            if (string.IsNullOrEmpty(stringValue))
                return new UniversalValue(string.Empty);

            return new UniversalValue(stringValue.Replace(oldValue, newValue));
        }

        public UniversalValue AddDays(decimal value)
        {
            if (Value is DateTime dateTime)
            {
                return new UniversalValue(dateTime.AddDays((double)value));
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                    return new UniversalValue(dateTime.AddDays((double)value));
            }

            return new UniversalValue();
        }

        public UniversalValue AddHours(decimal value)
        {
            if (Value is DateTime dateTime)
            {
                return new UniversalValue(dateTime.AddHours((double)value));
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                    return new UniversalValue(dateTime.AddHours((double)value));
            }

            return new UniversalValue();
        }

        public UniversalValue AddMinutes(decimal value)
        {
            if (Value is DateTime dateTime)
            {
                return new UniversalValue(dateTime.AddMinutes((double)value));
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                    return new UniversalValue(dateTime.AddMinutes((double)value));
            }

            return new UniversalValue();
        }

        public UniversalValue AddMonths(int months)
        {
            if (Value is DateTime dateTime)
            {
                return new UniversalValue(dateTime.AddMonths(months));
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                    return new UniversalValue(dateTime.AddMonths(months));
            }

            return new UniversalValue();
        }

        public UniversalValue AddSeconds(decimal value)
        {
            if (Value is DateTime dateTime)
            {
                return new UniversalValue(dateTime.AddSeconds((double)value));
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                    return new UniversalValue(dateTime.AddSeconds((double)value));
            }

            return new UniversalValue();
        }

        public UniversalValue AddYears(int value)
        {
            if (Value is DateTime dateTime)
            {
                return new UniversalValue(dateTime.AddYears(value));
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                    return new UniversalValue(dateTime.AddYears(value));
            }

            return new UniversalValue();
        }        

        public override string ToString()
        {
            if (Value != null)
                return Value.ToString();
            else
                return string.Empty;
        }

        public DateTime ToDate()
        {
            if (Value is DateTime dateTime)
            {
                return dateTime;
            }
            
            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                    return dateTime;
            }

            return DateTime.MinValue;
        }

        public UniversalValue AddQuarters(int quarter)
        {
            // if (Value is DateTime dateTime)
            // {
            //     return new UniversalValue(dateTime.AddMonths(quarter * 3));
            // }
            //
            // if (Value is string stringValue)
            // {
            //     if (DateTime.TryParse(stringValue, out dateTime))
            //         return new UniversalValue(dateTime.AddMonths(quarter * 3));
            // }

            var dateTime = ToDate();
            var result = new UniversalValue(dateTime.AddQuarters(quarter));

            return result;
        }

        public UniversalValue SetSecond(int second)
        {
            if (Value is DateTime dateTime)
            {
                var resDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, second);
                return new UniversalValue(resDate);
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var resDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, second);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue SetMinute(int minute)
        {
            if (Value is DateTime dateTime)
            {
                var resDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minute, dateTime.Second);
                return new UniversalValue(resDate);
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var resDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minute, dateTime.Second);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue SetHour(int hour)
        {
            if (Value is DateTime dateTime)
            {
                var resDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, dateTime.Minute, dateTime.Second);
                return new UniversalValue(resDate);
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var resDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, dateTime.Minute, dateTime.Second);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue SetDay(int day)
        {
            if (Value is DateTime dateTime)
            {
                var resDate = new DateTime(dateTime.Year, dateTime.Month, day, dateTime.Hour, dateTime.Minute, dateTime.Second);
                return new UniversalValue(resDate);
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var resDate = new DateTime(dateTime.Year, dateTime.Month, day, dateTime.Hour, dateTime.Minute, dateTime.Second);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue SetQuarter(int quarter)
        {
            int monthBegin;

            switch (quarter)
            {
                case 1:
                    monthBegin = 1;
                    break;
                case 2:
                    monthBegin = 4;
                    break;
                case 3:
                    monthBegin = 7;
                    break;
                case 4:
                    monthBegin = 10;
                    break;
                default:
                    monthBegin = 1;
                    break;
            }

            return SetMonth(monthBegin);
        }

        public UniversalValue SetMonth(int month)
        {
            if (Value is DateTime dateTime)
            {
                var resDate = new DateTime(dateTime.Year, month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
                return new UniversalValue(resDate);
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var resDate = new DateTime(dateTime.Year, month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue SetYear(int year)
        {
            if (Value is DateTime dateTime)
            {
                var resDate = new DateTime(year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
                return new UniversalValue(resDate);
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var resDate = new DateTime(year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue EndYear()
        {
            if (Value is DateTime dateTime)
            {
                var resDate = new DateTime(dateTime.Year, 12, 31, 23, 59, 59);
                return new UniversalValue(resDate);
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var resDate = new DateTime(dateTime.Year, 12, 31, 23, 59, 59);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue BeginYear()
        {
            if (Value is DateTime dateTime)
            {
                var resDate = new DateTime(dateTime.Year, 1, 1, 0, 0, 0);
                return new UniversalValue(resDate);
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var resDate = new DateTime(dateTime.Year, 1, 1, 0, 0, 0);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue BeginDay()
        {
            if (Value is DateTime dateTime)
            {
                var resDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
                return new UniversalValue(resDate);
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var resDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue EndDay()
        {
            if (Value is DateTime dateTime)
            {
                var resDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
                return new UniversalValue(resDate);
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var resDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue BeginMonth()
        {
            if (Value is DateTime dateTime)
            {
                var resDate = new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
                return new UniversalValue(resDate);
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var resDate = new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue EndMonth()
        {
            var tempUniversalValue = AddMonths(1);
            tempUniversalValue = tempUniversalValue.BeginMonth();
            tempUniversalValue = tempUniversalValue.AddDays(-1);

            if (tempUniversalValue.Value is DateTime dateTime)
            {
                var resDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
                return new UniversalValue(resDate);
            }

            if (tempUniversalValue.Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var resDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue BeginQuarter()
        {
            if (Value is DateTime dateTime)
            {
                var monthBegin = GetMonthBegin(dateTime.Month);
                var resDate = new DateTime(dateTime.Year, monthBegin, 1, 0, 0, 0);
                return new UniversalValue(resDate);
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var monthBegin = GetMonthBegin(dateTime.Month);
                    var resDate = new DateTime(dateTime.Year, monthBegin, 1, 0, 0, 0);
                    return new UniversalValue(resDate);
                }
            }

            return new UniversalValue();
        }

        public UniversalValue EndQuarter()
        {
            if (Value is DateTime dateTime)
            {
                var monthEnd = GetMonthEnd(dateTime.Month);
                var tempUniversalValue = new UniversalValue(new DateTime(dateTime.Year, monthEnd, 1, 0, 0, 0));
                return tempUniversalValue.EndMonth();
            }

            if (Value is string stringValue)
            {
                if (DateTime.TryParse(stringValue, out dateTime))
                {
                    var monthEnd = GetMonthEnd(dateTime.Month);
                    UniversalValue tempUniversalValue = new UniversalValue(new DateTime(dateTime.Year, monthEnd, 1, 0, 0, 0));
                    return tempUniversalValue.EndMonth();
                }
            }

            return new UniversalValue();
        }

        private static UniversalValue PerformOperation(UniversalValue v1,
            UniversalValue v2,
            Func<decimal, decimal, object> decimalOperation)
        {
            if (v1.Value is decimal || v2.Value is decimal)
            {
                if (v1.Value == null)
                {
                    return new UniversalValue(v2.Value);
                }
                else if (v2.Value == null)
                {
                    return new UniversalValue(v1.Value);
                }
                else if (v1.Value is int v1Int)
                {
                    return new UniversalValue(decimalOperation((decimal)v1Int, (decimal)v2.Value));
                }
                else if (v2.Value is int v2Int)
                {
                    return new UniversalValue(decimalOperation((decimal)v1.Value, (decimal)v2Int));
                }
                else
                {
                    return new UniversalValue(decimalOperation((decimal)v1.Value, (decimal)v2.Value));
                }
            }

            return new UniversalValue(v1.Value?.ToString() + v2.Value?.ToString());
        }

        private int GetMonthBegin(int dateMonth)
        {
            int monthBegin;

            switch (dateMonth)
            {
                case 1:
                case 2:
                case 3:
                    monthBegin = 1;
                    break;
                case 4:
                case 5:
                case 6:
                    monthBegin = 4;
                    break;
                case 7:
                case 8:
                case 9:
                    monthBegin = 7;
                    break;
                case 10:
                case 11:
                case 12:
                    monthBegin = 10;
                    break;
                default:
                    monthBegin = 1;
                    break;
            }

            return monthBegin;
        }
        private int GetMonthEnd(int dateMonth)
        {
            int monthEnd;

            switch (dateMonth)
            {
                case 1:
                case 2:
                case 3:
                    monthEnd = 3;
                    break;
                case 4:
                case 5:
                case 6:
                    monthEnd = 6;
                    break;
                case 7:
                case 8:
                case 9:
                    monthEnd = 9;
                    break;
                case 10:
                case 11:
                case 12:
                    monthEnd = 12;
                    break;
                default:
                    monthEnd = 3;
                    break;
            }

            return monthEnd;
        }
    }
}