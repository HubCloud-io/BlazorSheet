﻿using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetCell
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
        public Guid RowUid { get; set; }
        public Guid ColumnUid { get; set; }
        public Guid? StyleUid { get; set; }
        public Guid? EditSettingsUid { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }

        [JsonIgnore] public string Text { get; set; }
        public object Value { get; set; }
        public string Formula { get; set; }

        public bool HasLink => !string.IsNullOrEmpty(Link) && !string.IsNullOrWhiteSpace(Link);

        [JsonIgnore]
        public decimal DecimalValue
        {
            get
            {
                if (Value is decimal decimalValue)
                {
                    return decimalValue;
                }
                else
                {
                    return 0M;
                }
            }
            set => Value = value;
        }
        
        [JsonIgnore]
        public int IntValue
        {
            get
            {
                if (Value is int intValue)
                {
                    return intValue;
                }
                else
                {
                    return 0;
                }
            }
            set => Value = value;
        }
        
        [JsonIgnore]
        public bool BoolValue
        {
            get
            {
                if (Value is bool boolValue)
                {
                    return boolValue;
                }
                else
                {
                    return false;
                }
            }
            set => Value = value;
        }
        
        [JsonIgnore]
        public string StringValue
        {
            get => Value?.ToString() ?? string.Empty;
            set => Value = value;
        }

        [JsonIgnore]
        public DateTime DateTimeValue
        {
            get
            {
                if (Value is DateTime dateTime)
                {
                    return dateTime;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            set => Value = value;
        }

        public SheetCell ConcreteClone()
        {
            var clone = new SheetCell()
            {
                Uid = this.Uid,
                RowUid = this.RowUid,
                ColumnUid = this.ColumnUid,
                Name = this.Name,
                Link = this.Link,

                Text = this.Text,
                Value = this.Value
            };

            return clone;
        }

        public object Clone()
        {
            return ConcreteClone();
        }

        public void ApplyFormat(string format)
        {
            if (Value == null)
                return;

            var text = Value as string;

            if (string.IsNullOrEmpty(text) ||
                string.IsNullOrEmpty(format))
                return;

            if (DateTime.TryParse(text, out var date))
                Text = date.ToString(format);
            else if (decimal.TryParse(
                text.Replace(',', '.'),
                NumberStyles.AllowDecimalPoint,
                new NumberFormatInfo { NumberDecimalSeparator = "." },
                out var decimalValue))
                Text = decimalValue.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}