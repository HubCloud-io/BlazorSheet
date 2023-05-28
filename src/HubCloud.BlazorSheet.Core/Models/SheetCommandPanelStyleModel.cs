using System;
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

        public CellBorderTypes BorderType { get; set; }
        public int BorderWidth { get; set; } = 1;
        public string BorderColor { get; set; } = "#000000";

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