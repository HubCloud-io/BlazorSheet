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
            if (Value is string valueString)
                return new UniversalValue(valueString.Substring(startIndex));
            else
                return new UniversalValue(Value.ToString());
        }

        public UniversalValue Substring(int startIndex, int length)
        {
            if (Value is string valueString)
                return new UniversalValue(valueString.Substring(startIndex, length));
            else
                return new UniversalValue(Value.ToString());
        }

        public UniversalValue ToUpper()
        {
            if (Value is string valueString)
                return new UniversalValue(valueString.ToUpper());
            else
                return new UniversalValue(Value.ToString());
        }

        public UniversalValue ToLower()
        {
            if (Value is string valueString)
                return new UniversalValue(valueString.ToLower());
            else
                return new UniversalValue(Value.ToString());
        }

        public UniversalValue IndexOf(string value)
        {
            if (Value is string valueString)
                return new UniversalValue(valueString.IndexOf(value));
            else
                return new UniversalValue(Value.ToString());
        }

        public UniversalValue Replace(string oldValue, string newValue)
        {
            if (Value is string valueString)
                return new UniversalValue(valueString.Replace(oldValue, newValue));
            else
                return new UniversalValue(Value.ToString());
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
            return Value?.ToString();
        }
    }
}