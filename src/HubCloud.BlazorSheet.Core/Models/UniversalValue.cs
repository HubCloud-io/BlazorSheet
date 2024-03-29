﻿using HubCloud.BlazorSheet.Core.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Numerics;
using ExpressoFunctions.FunctionLibrary;

namespace HubCloud.BlazorSheet.Core.Models
{
    public struct UniversalValue : IStringMethods, IDateTimeMethods, IDateTimeProperties
    {
        public object Value { get; set; }

        public UniversalValue Day => new UniversalValue(ToDate().Day);

        public UniversalValue Hour => new UniversalValue(ToDate().Hour);

        public UniversalValue Month => new UniversalValue(ToDate().Month);

        public UniversalValue Minute => new UniversalValue(ToDate().Minute);

        public UniversalValue Second => new UniversalValue(ToDate().Second);

        public UniversalValue Year => new UniversalValue(ToDate().Year);

        public UniversalValue(object value)
        {
            Value = value;
        }

        public static UniversalValue operator +(UniversalValue v1, UniversalValue v2)
        {
            return PerformOperation(v1,
                v2,
                (a, b) => a + b,
                (a, b) => a + b);
        }

        public static UniversalValue operator +(object v1, UniversalValue v2)
        {
            return PerformOperation(new UniversalValue(v1),
                v2,
                (a, b) => a + b,
                (a, b) => a + b);
        }

        public static UniversalValue operator +(UniversalValue v1, object v2)
        {
            return PerformOperation(v1,
                new UniversalValue(v2),
                (a, b) => a + b,
                (a, b) => a + b);
        }

        public static UniversalValue operator -(UniversalValue v1, UniversalValue v2)
        {
            return PerformOperation(v1,
                v2,
                (a, b) => a - b,
                (a, b) => a - b);
        }

        public static UniversalValue operator -(UniversalValue v1, object v2)
        {
            return PerformOperation(v1,
                new UniversalValue(v2),
                (a, b) => a - b,
                (a, b) => a - b);
        }

        public static UniversalValue operator -(object v1, UniversalValue v2)
        {
            return PerformOperation(new UniversalValue(v1),
                v2,
                (a, b) => a - b,
                (a, b) => a - b);
        }

        public static UniversalValue operator *(UniversalValue v1, UniversalValue v2)
        {
            return PerformOperation(v1,
                v2,
                (a, b) => a * b,
                (a, b) => a * b);
        }

        public static UniversalValue operator *(UniversalValue v1, object v2)
        {
            return PerformOperation(v1,
                new UniversalValue(v2),
                (a, b) => a * b,
                (a, b) => a * b);
        }

        public static UniversalValue operator *(object v1, UniversalValue v2)
        {
            return PerformOperation(new UniversalValue(v1),
                v2,
                (a, b) => a * b,
                (a, b) => a * b);
        }

        public static UniversalValue operator /(UniversalValue v1, UniversalValue v2)
        {
            return PerformOperation(v1,
                v2,
                (a, b) => a / b,
                (a, b) => a / b);
        }

        public static UniversalValue operator /(UniversalValue v1, object v2)
        {
            return PerformOperation(v1,
                new UniversalValue(v2),
                (a, b) => a / b,
                (a, b) => a / b);
        }

        public static UniversalValue operator /(object v1, UniversalValue v2)
        {
            return PerformOperation(new UniversalValue(v1),
                v2,
                (a, b) => a / b,
                (a, b) => a / b);
        }

        public static UniversalValue operator >(UniversalValue v1, UniversalValue v2)
        {
            return PerformComparisonOperation(v1,
                v2,
                (a, b) => a > b,
                (a, b) => a > b, 
                (a, b) => a > b);
        }

        public static UniversalValue operator >(UniversalValue v1, object v2)
        {
            return PerformComparisonOperation(v1,
                new UniversalValue(v2), (a, b) => a > b,
                (a, b) => a > b, 
                (a, b) => a > b);
        }

        public static UniversalValue operator >(object v1, UniversalValue v2)
        {
            return PerformComparisonOperation(new UniversalValue(v1),
                v2,
                (a, b) => a > b,
                (a, b) => a > b, 
                (a, b) => a > b);
        }

        public static UniversalValue operator <(UniversalValue v1, UniversalValue v2)
        {
            return PerformComparisonOperation(v1,
                v2,
                (a, b) => a < b,
                (a, b) => a < b, 
                (a, b) => a < b);
        }

        public static UniversalValue operator <(UniversalValue v1, object v2)
        {
            return PerformComparisonOperation(v1,
                new UniversalValue(v2),
                (a, b) => a < b,
                (a, b) => a < b, 
                (a, b) => a < b);
        }

        public static UniversalValue operator <(object v1, UniversalValue v2)
        {
            return PerformComparisonOperation(new UniversalValue(v1),
                v2, (a, b) => a < b,
                (a, b) => a < b, 
                (a, b) => a < b);
        }

        public static UniversalValue operator >=(UniversalValue v1, UniversalValue v2)
        {
            return PerformComparisonOperation(v1,
                v2, (a, b) => a >= b,
                (a, b) => a >= b, 
                (a, b) => a >= b);
        }

        public static UniversalValue operator >=(UniversalValue v1, object v2)
        {
            return PerformComparisonOperation(v1,
                new UniversalValue(v2),
                (a, b) => a >= b,
                (a, b) => a >= b, 
                (a, b) => a >= b);
        }

        public static UniversalValue operator >=(object v1, UniversalValue v2)
        {
            return PerformComparisonOperation(new UniversalValue(v1),
                v2, (a, b) => a >= b,
                (a, b) => a >= b, 
                (a, b) => a >= b);
        }

        public static UniversalValue operator <=(UniversalValue v1, UniversalValue v2)
        {
            return PerformComparisonOperation(v1,
                v2,
                (a, b) => a <= b,
                (a, b) => a <= b, 
                (a, b) => a <= b);
        }

        public static UniversalValue operator <=(UniversalValue v1, object v2)
        {
            return PerformComparisonOperation(v1,
                new UniversalValue(v2), (a, b) => a <= b,
                (a, b) => a <= b, 
                (a, b) => a <= b);
        }

        public static UniversalValue operator <=(object v1, UniversalValue v2)
        {
            return PerformComparisonOperation(new UniversalValue(v1),
                v2, (a, b) => a <= b,
                (a, b) => a <= b, 
                (a, b) => a <= b);
        }

        public static UniversalValue operator ==(UniversalValue v1, UniversalValue v2)
        {
            return PerformComparisonOperation(v1,
                v2,
                (a, b) => a == b,
                (a, b) => a == b, 
                (a, b) => a == b);
        }

        public static UniversalValue operator ==(UniversalValue v1, object v2)
        {
            return PerformComparisonOperation(v1, 
                new UniversalValue(v2), 
                (a, b) => a == b, 
                (a, b) => a == b, 
                (a, b) => a == b);
        }

        public static UniversalValue operator ==(object v1, UniversalValue v2)
        {
            return PerformComparisonOperation(new UniversalValue(v1), 
                v2, (a, b) => a == b, 
                (a, b) => a == b, 
                (a, b) => a == b);
        }

        public static UniversalValue operator !=(UniversalValue v1, UniversalValue v2)
        {
            return PerformComparisonOperation(v1, 
                v2, (a, b) => a != b, 
                (a, b) => a != b, 
                (a, b) => a != b);
        }

        public static UniversalValue operator !=(UniversalValue v1, object v2)
        {
            return PerformComparisonOperation(v1, 
                new UniversalValue(v2), (a, b) => a != b, 
                (a, b) => a != b, 
                (a, b) => a != b);
        }

        public static UniversalValue operator !=(object v1, UniversalValue v2)
        {
            return PerformComparisonOperation(new UniversalValue(v1), 
                v2, (a, b) => a != b, 
                (a, b) => a != b, 
                (a, b) => a != b);
        }

        public static UniversalValue operator &(UniversalValue v1, UniversalValue v2)
        {
            return AndOrOperationResult(v1, v2, (a, b) => a & b);
        }

        public static UniversalValue operator |(UniversalValue v1, UniversalValue v2)
        {
            return AndOrOperationResult(v1, v2, (a, b) => a | b);
        }

        private static UniversalValue AndOrOperationResult(UniversalValue v1, UniversalValue v2,
            Func<bool?, bool?, bool?> func)
        {
            if (bool.TryParse(v1.ToString(), out bool v1Bool))
            {
                if (bool.TryParse(v2.ToString(), out bool v2Bool))
                    return new UniversalValue(func(v1Bool, v2Bool));
                else
                    return new UniversalValue(func(v1Bool, null));
            }
            else
            {
                if (bool.TryParse(v2.ToString(), out bool v2Bool))
                    return new UniversalValue(func(null, v2Bool));
                else
                    return new UniversalValue();
            }
        }

        public static bool operator true(UniversalValue uv)
        {
            // if (bool.TryParse(uv.ToString(), out bool valBool))
            //     return valBool;
            //
            // return false;
            return uv.ToBool();
        }
        
        public static bool operator false(UniversalValue uv)
        {
            return false;
           //return uv.ToBool();
        }
        
        public static implicit operator bool(UniversalValue uv)
        {
            if (uv.Value is bool boolValue)
            {
                return boolValue;
            }
        
            return IsNotEmptyFunction.Eval(uv.Value);
        }
        
        public static implicit operator UniversalValue(bool boolValue)
        {
            return new UniversalValue(boolValue);
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
            var dateTime = ToDate();
            return new UniversalValue(dateTime.AddDays(value));
        }

        public UniversalValue AddHours(decimal value)
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.AddHours(value));
        }

        public UniversalValue AddMinutes(decimal value)
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.AddMinutes(value));
        }

        public UniversalValue AddMonths(int months)
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.AddMonths(months));
        }

        public UniversalValue AddSeconds(decimal value)
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.AddSeconds(value));
        }

        public UniversalValue AddYears(int value)
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.AddYears(value));
        }

        public override bool Equals(object obj)
        {
            if (Value == null || obj == null)
                return false;
            else
            {
                if (obj is UniversalValue uv)
                    return Value.Equals(uv.Value);
                else
                    return Value.Equals(obj);
            }
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

        public bool ToBool()
        {
            if (Value is bool boolValue)
            {
                return boolValue;
            }

            return IsNotEmptyFunction.Eval(Value);
        }

        public UniversalValue AddQuarters(int quarter)
        {
            var dateTime = ToDate();
            var result = new UniversalValue(dateTime.AddQuarters(quarter));

            return result;
        }

        public UniversalValue SetSecond(int second)
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.SetSecond(second));
        }

        public UniversalValue SetMinute(int minute)
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.SetMinute(minute));
        }

        public UniversalValue SetHour(int hour)
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.SetHour(hour));
        }

        public UniversalValue SetDay(int day)
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.SetDay(day));
        }

        public UniversalValue SetQuarter(int quarter)
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.SetQuarter(quarter));
        }

        public UniversalValue SetMonth(int month)
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.SetMonth(month));
        }

        public UniversalValue SetYear(int year)
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.SetYear(year));
        }

        public UniversalValue EndYear()
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.EndYear());
        }

        public UniversalValue BeginYear()
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.BeginYear());
        }

        public UniversalValue BeginDay()
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.BeginDay());
        }

        public UniversalValue EndDay()
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.EndDay());
        }

        public UniversalValue BeginMonth()
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.BeginMonth());
        }

        public UniversalValue EndMonth()
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.EndMonth());
        }

        public UniversalValue BeginQuarter()
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.BeginQuarter());
        }

        public UniversalValue EndQuarter()
        {
            var dateTime = ToDate();
            return new UniversalValue(dateTime.EndQuarter());
        }

        private static UniversalValue PerformOperation(UniversalValue v1,
            UniversalValue v2,
            Func<decimal, decimal, object> decimalOperation,
            Func<int, int, object> intOperation)
        {
            if (string.IsNullOrEmpty(v1.Value?.ToString()))
            {
                return v2;
            }

            if (string.IsNullOrEmpty(v2.Value?.ToString()))
            {
                return v1;
            }

            if (v1.Value is decimal || v2.Value is decimal)
            {
                if (v1.Value is int v1Int)
                {
                    return new UniversalValue(decimalOperation((decimal) v1Int, (decimal) v2.Value));
                }

                if (v1.Value is float v1Float)
                {
                    return new UniversalValue(decimalOperation((decimal) v1Float, (decimal) v2.Value));
                }

                if (v1.Value is double v1Double)
                {
                    return new UniversalValue(decimalOperation((decimal) v1Double, (decimal) v2.Value));
                }

                if (v2.Value is int v2Int)
                {
                    return new UniversalValue(decimalOperation((decimal) v1.Value, (decimal) v2Int));
                }

                if (v2.Value is float v2Float)
                {
                    return new UniversalValue(decimalOperation((decimal) v1.Value, (decimal) v2Float));
                }

                if (v2.Value is double v2Double)
                {
                    return new UniversalValue(decimalOperation((decimal) v1.Value, (decimal) v2Double));
                }


                return new UniversalValue(decimalOperation((decimal) v1.Value, (decimal) v2.Value));
            }

            if (v1.Value is int || v2.Value is int)
            {
                return new UniversalValue(intOperation((int) v1.Value, (int) v2.Value));
            }

            return new UniversalValue(v1.Value?.ToString() + v2.Value?.ToString());
        }

        private static UniversalValue PerformComparisonOperation(UniversalValue v1, UniversalValue v2,
            Func<decimal, decimal, bool> decimalOperation,
            Func<int, int, bool> intOperation, 
            Func<DateTime, DateTime, bool> dateTimeOperation)
        {
            
            if (string.IsNullOrEmpty(v1.Value?.ToString()))
            {
                return new UniversalValue(false);
            }

            if (string.IsNullOrEmpty(v2.Value?.ToString()))
            {
                return new UniversalValue(false);
            }
            
            if (v1.Value is decimal || v2.Value is decimal)
            {
                if (v1.Value is int v1Int)
                {
                    return new UniversalValue(decimalOperation((decimal) v1Int, (decimal) v2.Value));
                }

                if (v1.Value is float v1Float)
                {
                    return new UniversalValue(decimalOperation((decimal) v1Float, (decimal) v2.Value));
                }

                if (v1.Value is double v1Double)
                {
                    return new UniversalValue(decimalOperation((decimal) v1Double, (decimal) v2.Value));
                }

                if (v2.Value is int v2Int)
                {
                    return new UniversalValue(decimalOperation((decimal) v1.Value, (decimal) v2Int));
                }

                if (v2.Value is float v2Float)
                {
                    return new UniversalValue(decimalOperation((decimal) v1.Value, (decimal) v2Float));
                }

                if (v2.Value is double v2Double)
                {
                    return new UniversalValue(decimalOperation((decimal) v1.Value, (decimal) v2Double));
                }


                return new UniversalValue(decimalOperation((decimal) v1.Value, (decimal) v2.Value));
            }

            if (v1.Value is int || v2.Value is int)
            {
                return new UniversalValue(intOperation((int) v1.Value, (int) v2.Value));
            }

            if (v1.Value is DateTime && v2.Value is DateTime)
            {
                return new UniversalValue(dateTimeOperation((DateTime)v1.Value, (DateTime)v2.Value));
            }

            return new UniversalValue(false);
        }
    }
}