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