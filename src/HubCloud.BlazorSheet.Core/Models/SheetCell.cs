using System;
using System.Globalization;
using System.Xml.Serialization;
using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Enums;
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
        public int Colspan { get; set; } = 1;
        public int Rowspan { get; set; } = 1;
        public bool HiddenByJoin { get; set; }
        public bool Locked { get; set; } = true;
        public int Indent { get; set; }

        [JsonIgnore] public string Text { get; set; }
        public object Value { get; set; }
        public string Formula { get; set; }
        public string Format { get; set; } = string.Empty;

        public bool HasLink => !string.IsNullOrEmpty(Link) && !string.IsNullOrWhiteSpace(Link);

        public CellKey GetKey()
        {
            return new CellKey(RowUid, ColumnUid);
        }

        [XmlIgnore]
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

        [XmlIgnore]
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

        [XmlIgnore]
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

        [XmlIgnore]
        [JsonIgnore]
        public string StringValue
        {
            get => Value?.ToString() ?? string.Empty;
            set => Value = value;
        }

        [XmlIgnore]
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
                Indent = this.Indent,

                Text = this.Text,
                Value = this.Value
            };

            return clone;
        }

        public object Clone()
        {
            return ConcreteClone();
        }

        public void ApplyFormat()
        {
            if (Value == null)
                return;

            if (string.IsNullOrEmpty(StringValue) ||
                string.IsNullOrEmpty(Format))
                return;

            if (DateTime.TryParse(StringValue, out var date))
                Text = date.ToString(Format);
            else if (decimal.TryParse(
                StringValue.Replace(',', '.'),
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                new NumberFormatInfo { NumberDecimalSeparator = "." },
                out var decimalValue))
                Text = decimalValue.ToString(Format, CultureInfo.InvariantCulture);
        }

        public void SetFormat(CellFormatTypes formatType, string customFormat)
        {
            switch (formatType)
            {
                case CellFormatTypes.Custom:
                    Format = customFormat;
                    break;
                case CellFormatTypes.None:
                    Format = CellFormatConsts.None;
                    break;
                case CellFormatTypes.Integer:
                    Format = CellFormatConsts.Integer;
                    break;
                case CellFormatTypes.IntegerTwoDecimalPlaces:
                    Format = CellFormatConsts.IntegerTwoDecimalPlaces;
                    break;
                case CellFormatTypes.IntegerThreeDecimalPlaces:
                    Format = CellFormatConsts.IntegerThreeDecimalPlaces;
                    break;
                case CellFormatTypes.Date:
                    Format = CellFormatConsts.Date;
                    break;
                case CellFormatTypes.DateTime:
                    Format = CellFormatConsts.DateTime;
                    break;
                default:
                    Format = string.Empty;
                    break;
            }
        }
    }
}