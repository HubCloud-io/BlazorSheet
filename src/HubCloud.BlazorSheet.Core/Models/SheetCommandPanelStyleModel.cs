using System;
using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Enums;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetCommandPanelStyleModel
    {
        public const int DefaultFontSize = 12;
        public const string DefaultBackgroundColor = "#ffffff";
        public const string DefaultColor = "#000000";
        public const string DefaultTextAlign = "left";
        
        public string BackgroundColor { get; set; } = DefaultBackgroundColor;
        public string Color { get; set; } = DefaultColor;
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public int FontSize { get; set; }
        public string FontFamily { get; set; }
        public string TextAlign { get; set; } = DefaultTextAlign;
        public string CustomFormat { get; set; }
        public CellFormatTypes FormatType { get; set; } = CellFormatTypes.None;

        public CellBorderTypes BorderType { get; set; }
        public int BorderWidth { get; set; } = 1;
        public string BorderColor { get; set; } = "#000000";
        
        public CellControlKinds ControlKind { get; set; }
        public int NumberDigits { get; set; }

        public void CopyFrom(SheetCommandPanelStyleModel clone)
        {
            if (clone == null)
            {
                return;
            }

            this.BackgroundColor = clone.BackgroundColor;
            this.Color = clone.Color;
            this.IsBold = clone.IsBold;
            this.IsItalic = clone.IsItalic;
            this.FontSize = clone.FontSize;
            this.FontFamily = clone.FontFamily;
            this.BorderWidth = clone.BorderWidth;
            this.BorderColor = clone.BorderColor;
            this.FormatType = clone.FormatType;
            this.CustomFormat = clone.CustomFormat;
        }
        
        public void CopyFrom(SheetCellStyle cellStyle)
        {
            if (cellStyle == null)
            {
                return;
            }
        
            this.BackgroundColor = cellStyle.BackgroundColor;
            this.Color = cellStyle.Color;

            if (!string.IsNullOrEmpty(cellStyle.FontWeight))
            {
                this.IsBold = cellStyle.FontWeight.Equals("bold", StringComparison.OrdinalIgnoreCase) ? true : false;
            }
            else
            {
                IsBold = false;
            }
        
            if (!string.IsNullOrEmpty(cellStyle.FontStyle))
            {
                this.IsItalic = cellStyle.FontStyle.Equals("italic", StringComparison.OrdinalIgnoreCase) ? true : false;
            }
            else
            {
                IsItalic = false;
            }
        
            this.FontSize = ParseFontSize(cellStyle.FontSize);
            this.FontFamily = cellStyle.FontFamily;

            if (!string.IsNullOrEmpty(cellStyle.TextAlign))
            {
                TextAlign = cellStyle.TextAlign;
            }
            else
            {
                TextAlign = DefaultTextAlign;
            }

            switch (cellStyle.Format)
            {
                case "":
                case null:
                    this.FormatType = CellFormatTypes.None;
                    break;
                case CellFormatConsts.Integer:
                    this.FormatType = CellFormatTypes.Integer;
                    break;
                case CellFormatConsts.IntegerTwoDecimalPlaces:
                    this.FormatType = CellFormatTypes.IntegerTwoDecimalPlaces;
                    break;
                case CellFormatConsts.IntegerThreeDecimalPlaces:
                    this.FormatType = CellFormatTypes.IntegerThreeDecimalPlaces;
                    break;
                case CellFormatConsts.Date:
                    this.FormatType = CellFormatTypes.Date;
                    break;
                case CellFormatConsts.DateTime:
                    this.FormatType = CellFormatTypes.DateTime;
                    break;
                default:
                    this.FormatType = CellFormatTypes.Custom;
                    this.CustomFormat = cellStyle.Format;
                    break;
            }
        }

        public void SetEditSettings(SheetCellEditSettings editSettings)
        {
            ControlKind = editSettings.ControlKind;
            NumberDigits = editSettings.NumberDigits;
        }

        private int ParseFontSize(string fontSizePx)
        {
            if (string.IsNullOrEmpty(fontSizePx))
            {
                return DefaultFontSize;
            }

            var fontSizeStr = fontSizePx.Replace("px", "");

            if (int.TryParse(fontSizeStr, out var result))
            {
                return result;
            }
            else
            {
                return DefaultFontSize;
            }
        }
    }
}