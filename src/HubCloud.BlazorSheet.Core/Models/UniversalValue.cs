using Newtonsoft.Json.Linq;
using System;
using System.Numerics;

namespace HubCloud.BlazorSheet.Core.Models
{
    public struct UniversalValue
    {
        public object Value { get; set; }

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
                    return new UniversalValue(decimalOperation((decimal) v1Int, (decimal) v2.Value));
                }
                else if (v2.Value is int v2Int)
                {
                    return new UniversalValue(decimalOperation((decimal) v1.Value, (decimal) v2Int));
                }
                else
                {
                    return new UniversalValue(decimalOperation((decimal) v1.Value, (decimal) v2.Value));
                }
            }

            return new UniversalValue(v1.Value?.ToString() + v2.Value?.ToString());
        }


        public override string ToString()
        {
            if (Value != null)
                return Value.ToString();
            else
                return string.Empty;
        }
    }
}