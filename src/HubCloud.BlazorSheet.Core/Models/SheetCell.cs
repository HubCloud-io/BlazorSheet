﻿using System;
using System.Globalization;
using System.Xml.Serialization;
using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Enums;
using HubCloud.BlazorSheet.Core.Helpers;
using Newtonsoft.Json;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetCell
    {
        private string _text;
        private object _value;
        private bool _validationFailed;
        private bool _isSelected;

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

        [JsonIgnore]
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                ShouldRender = true;
            }
        }

        [JsonIgnore] public bool ShouldRender { get; set; }

        public object Value
        {
            get => _value;
            set
            {
                _value = value;
                ShouldRender = true;
            }
        }

        public string Formula { get; set; }
        public string Format { get; set; } = string.Empty;

        [JsonIgnore]
        public bool ValidationFailed
        {
            get => _validationFailed;
            set
            {
                _validationFailed = value;
                ShouldRender = true;
            }
        }
        
        [JsonIgnore]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                ShouldRender = true;
            }
        }

        [JsonIgnore] public string HtmlClass => CellClass();

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

        //public void ApplyFormat(int cellDataType = 0)
        //{
        //    if (cellDataType >= 20 ||
        //        string.IsNullOrEmpty(StringValue))
        //        return;

        //    if (string.IsNullOrEmpty(Format))
        //        Text = StringValue;

        //    if (decimal.TryParse(
        //            StringValue.Replace(',', '.'),
        //            NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
        //            new NumberFormatInfo { NumberDecimalSeparator = "." },
        //            out var decimalValue))
        //        Text = CellValueFormatHelper.ToStringWithFormat(decimalValue, Format);
        //    else if (DateTime.TryParse(StringValue, out var date))
        //        Text = date.ToString(Format);
        //}

        public void ApplyFormat(int cellDataType = 0)
        {
            if (cellDataType >= 20 ||
                string.IsNullOrEmpty(StringValue))
                return;

            if (Value is decimal decimalValue)
            {
                if (string.IsNullOrEmpty(Format))
                    Text = decimalValue.ToString(new NumberFormatInfo { NumberDecimalSeparator = "." });
                else
                    Text = CellValueFormatHelper.ToStringWithFormat(decimalValue, Format);
            }
            else if (Value is double doublelValue)
            {
                if (string.IsNullOrEmpty(Format))
                    Text = doublelValue.ToString(new NumberFormatInfo { NumberDecimalSeparator = "." });
                else
                    Text = CellValueFormatHelper.ToStringWithFormat(Convert.ToDecimal(doublelValue), Format);
            }
            else if (Value is DateTime dateTimeValue)
            {
                if (string.IsNullOrEmpty(Format))
                    Text = dateTimeValue.ToString(CellDisplayFormatConsts.Date);
                else
                    Text = dateTimeValue.ToString(Format);
            }
            else
            {
                if (string.IsNullOrEmpty(Format))
                    Text = StringValue;
                else
                    Text = CellValueFormatHelper.ToStringWithFormat(StringValue, Format);
            }
        }

        public void SetFormat(CellFormatTypes formatType, string customFormat)
        {
            switch (formatType)
            {
                case CellFormatTypes.Custom:
                    Format = customFormat;
                    break;
                case CellFormatTypes.None:
                    Format = CellDisplayFormatConsts.None;
                    break;
                case CellFormatTypes.Integer:
                    Format = CellDisplayFormatConsts.Integer;
                    break;
                case CellFormatTypes.IntegerTwoDecimalPlaces:
                    Format = CellDisplayFormatConsts.IntegerTwoDecimalPlaces;
                    break;
                case CellFormatTypes.IntegerThreeDecimalPlaces:
                    Format = CellDisplayFormatConsts.IntegerThreeDecimalPlaces;
                    break;
                case CellFormatTypes.IntegerWithSpaces:
                    Format = CellDisplayFormatConsts.IntegerWithSpaces;
                    break;
                case CellFormatTypes.IntegerWithSpacesTwoDecimalPlaces:
                    Format = CellDisplayFormatConsts.IntegerWithSpacesTwoDecimalPlaces;
                    break;
                case CellFormatTypes.IntegerWithSpacesThreeDecimalPlaces:
                    Format = CellDisplayFormatConsts.IntegerWithSpacesThreeDecimalPlaces;
                    break;
                case CellFormatTypes.IntegerNegativeWithSpaces:
                    Format = CellDisplayFormatConsts.IntegerNegativeWithSpaces;
                    break;
                case CellFormatTypes.IntegerNegativeWithSpacesTwoDecimalPlaces:
                    Format = CellDisplayFormatConsts.IntegerNegativeWithSpacesTwoDecimalPlaces;
                    break;
                case CellFormatTypes.IntegerNegativeWithSpacesThreeDecimalPlaces:
                    Format = CellDisplayFormatConsts.IntegerNegativeWithSpacesThreeDecimalPlaces;
                    break;
                case CellFormatTypes.Date:
                    Format = CellDisplayFormatConsts.Date;
                    break;
                case CellFormatTypes.DateTime:
                    Format = CellDisplayFormatConsts.DateTime;
                    break;
                default:
                    Format = string.Empty;
                    break;
            }
        }
        
        private string CellClass()
        {
            var result = "hc-sheet-cell";

            if (ValidationFailed)
                return result += " hc-sheet-cell__non-valid";

            if (IsSelected)
                return result += " hc-sheet-cell__active";

            return result;
        }
    }
}