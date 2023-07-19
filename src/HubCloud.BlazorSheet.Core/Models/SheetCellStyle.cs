using System;
using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Enums;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetCellStyle
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
        public string BackgroundColor { get; set; } = "#ffffff";
        public string Color { get; set; } = "#000000";
        public string FontWeight { get; set; } = "normal";
        public string FontStyle { get; set; } = "normal";
        public string FontSize { get; set; } = "12px";
        public string FontFamily { get; set; } = string.Empty;

        public string BorderLeft { get; set; } = string.Empty;
        public string BorderRight { get; set; } = string.Empty;
        public string BorderTop { get; set; } = string.Empty;
        public string BorderBottom { get; set; } = string.Empty;
        
        public string TextAlign { get; set; } = string.Empty;

        public string Format { get; set; } = string.Empty;

        public bool Locked { get; set; }

        public SheetCellStyle()
        {
            
        }

        public SheetCellStyle(SheetCommandPanelModel style)
        {
            SetStyle(style);
        }
        
        public void SetStyle(SheetCommandPanelModel style)
        {
            this.BackgroundColor = style.BackgroundColor;
            this.Color = style.Color;
            
            this.FontWeight = style.IsBold ? "bold":"normal";
            this.FontStyle = style.IsItalic ? "italic" : "normal";
            this.FontFamily = style.FontFamily;
            this.FontSize = $"{style.FontSize}px";
            this.TextAlign = style.TextAlign;

            switch (style.BorderType)
            {
                case CellBorderTypes.Top:
                    BorderTop = BorderStyle(style.BorderWidth, style.BorderColor);
                    break;
                case CellBorderTypes.Left:
                    BorderLeft = BorderStyle(style.BorderWidth, style.BorderColor);
                    break;
                case CellBorderTypes.Bottom:
                    BorderBottom = BorderStyle(style.BorderWidth, style.BorderColor);
                    break;
                case CellBorderTypes.Right:
                    BorderRight = BorderStyle(style.BorderWidth, style.BorderColor);
                    break;
                case CellBorderTypes.All:
                    BorderTop = BorderStyle(style.BorderWidth, style.BorderColor);
                    BorderLeft = BorderStyle(style.BorderWidth, style.BorderColor);
                    BorderBottom = BorderStyle(style.BorderWidth, style.BorderColor);
                    BorderRight = BorderStyle(style.BorderWidth, style.BorderColor);
                    break;
                case CellBorderTypes.None:
                    BorderTop = "";
                    BorderLeft = "";
                    BorderBottom = "";
                    BorderRight = "";
                    break;
            }

            switch (style.FormatType)
            {
                case CellFormatTypes.Custom:
                    this.Format = style.CustomFormat;
                    break;
                case CellFormatTypes.None:
                    this.Format = CellFormatConsts.None;
                    break;
                case CellFormatTypes.Integer:
                    this.Format = CellFormatConsts.Integer;
                    break;
                case CellFormatTypes.IntegerTwoDecimalPlaces:
                    this.Format = CellFormatConsts.IntegerTwoDecimalPlaces;
                    break;
                case CellFormatTypes.IntegerThreeDecimalPlaces:
                    this.Format = CellFormatConsts.IntegerThreeDecimalPlaces;
                    break;
                case CellFormatTypes.Date:
                    this.Format = CellFormatConsts.Date;
                    break;
                case CellFormatTypes.DateTime:
                    this.Format = CellFormatConsts.DateTime;
                    break;
                default:
                    this.Format = string.Empty;
                    break;
            }
        }
        
        public bool IsStyleEqual(SheetCellStyle other)
        {
            if (other == null)
                return false;

            var isEqual =
                string.Equals(BackgroundColor, other.BackgroundColor, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(Color, other.Color, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(FontWeight, other.FontWeight, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(FontStyle, other.FontStyle, StringComparison.OrdinalIgnoreCase)  &&
                string.Equals(FontSize, other.FontSize, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(FontFamily, other.FontFamily, StringComparison.OrdinalIgnoreCase)  &&
                string.Equals(BorderLeft, other.BorderLeft, StringComparison.OrdinalIgnoreCase)  &&
                string.Equals(BorderRight, other.BorderRight, StringComparison.OrdinalIgnoreCase)  &&
                string.Equals(BorderTop, other.BorderTop, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(BorderBottom, other.BorderBottom, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(TextAlign, other.TextAlign, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(Format, other.TextAlign, StringComparison.OrdinalIgnoreCase);

            return isEqual;
        }
        
        private string BorderStyle(int borderWidth, string borderColor)
        {
            var style = $"{borderWidth}px solid {borderColor}";
            return style;
        }
    }
}