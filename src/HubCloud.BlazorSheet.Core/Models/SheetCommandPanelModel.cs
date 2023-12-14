using System;
using System.Collections.Generic;
using System.Linq;
using HubCloud.BlazorSheet.Core.Consts;
using HubCloud.BlazorSheet.Core.Enums;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetCommandPanelModel
    {
        public const int DefaultFontSize = 12;
        public const string DefaultColor = SheetConsts.BlackColor;
        
        public string BackgroundColor { get; set; } = SheetConsts.WhiteBackground;
        public string Color { get; set; } = DefaultColor;
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public int FontSize { get; set; }
        public int FreezedRows { get; set; }
        public int FreezedColumns { get; set; }
        public string FontFamily { get; set; }
        public string TextAlign { get; set; } = SheetConsts.TextAlignLeft;
        public string CustomFormat { get; set; }
        public CellFormatTypes FormatType { get; set; } = CellFormatTypes.None;
        public CellBorderTypes BorderType { get; set; } = CellBorderTypes.None;
        public int BorderWidth { get; set; } = 1;
        public string BorderColor { get; set; } = SheetConsts.BlackColor;
        
        // public int DataType { get; set; }
        // public string ItemsSource { get; set; }
        // public CellControlKinds ControlKind { get; set; }
        // public int NumberDigits { get; set; }

        public SheetCellEditSettings EditSettings { get; set; } = new SheetCellEditSettings();
        
        public string SelectedCellAddress { get; set; }
        public string InputText { get; set; }
        public bool SheetProtected { get; set; }

        public void CopyFrom(SheetCommandPanelModel clone)
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
            this.SheetProtected = clone.SheetProtected;
        }
        
        public void CopyFrom(SheetCellStyle cellStyle)
        {
            if (cellStyle == null)
                return;

            this.BackgroundColor = cellStyle.BackgroundColor;
            this.Color = cellStyle.Color;

            if (!string.IsNullOrEmpty(cellStyle.FontWeight))
                this.IsBold = cellStyle.FontWeight.Equals("bold", StringComparison.OrdinalIgnoreCase) ? true : false;
            else
                IsBold = false;

            if (!string.IsNullOrEmpty(cellStyle.FontStyle))
                this.IsItalic = cellStyle.FontStyle.Equals("italic", StringComparison.OrdinalIgnoreCase) ? true : false;
            else
                IsItalic = false;

            this.FontSize = ParseFontSize(cellStyle.FontSize);
            this.FontFamily = cellStyle.FontFamily;

            if (!string.IsNullOrEmpty(cellStyle.TextAlign))
                TextAlign = cellStyle.TextAlign;
            else
                TextAlign = SheetConsts.TextAlignLeft;

            if (!string.IsNullOrEmpty(cellStyle.BorderBottom) &&
                !string.IsNullOrEmpty(cellStyle.BorderLeft) &&
                !string.IsNullOrEmpty(cellStyle.BorderRight) &&
                !string.IsNullOrEmpty(cellStyle.BorderTop))
            {
                BorderType = CellBorderTypes.All;
                SetBorderWidthColor(new List<string>
                {
                    cellStyle.BorderBottom,
                    cellStyle.BorderLeft,
                    cellStyle.BorderRight,
                    cellStyle.BorderTop
                });
            }
            else
            {
                if (!string.IsNullOrEmpty(cellStyle.BorderBottom))
                {
                    BorderType = CellBorderTypes.Bottom;
                    SetBorderWidthColor(cellStyle.BorderBottom);
                }
                else if (!string.IsNullOrEmpty(cellStyle.BorderLeft))
                {
                    BorderType = CellBorderTypes.Left;
                    SetBorderWidthColor(cellStyle.BorderLeft);
                }
                else if (!string.IsNullOrEmpty(cellStyle.BorderRight))
                {
                    BorderType = CellBorderTypes.Right;
                    SetBorderWidthColor(cellStyle.BorderRight);
                }
                else if (!string.IsNullOrEmpty(cellStyle.BorderTop))
                {
                    BorderType = CellBorderTypes.Top;
                    SetBorderWidthColor(cellStyle.BorderTop);
                }
                else
                {
                    BorderType = CellBorderTypes.None;
                    BorderWidth = 1;
                    BorderColor = SheetConsts.BlackColor;
                }
            }
        }

        private void SetBorderWidthColor(List<string> htmlBorders)
        {
            BorderWidth = 1;
            BorderColor = SheetConsts.BlackColor;

            var thicknessList = new List<int>();
            var colorList = new List<string>();

            foreach (var htmlBorder in htmlBorders)
            {
                var borderParams = htmlBorder.Split(new char[] { ' ' });
                if (borderParams.Length == 3)
                {
                    var thicknessPX = borderParams[0];
                    var thicknessStr = thicknessPX.Substring(0, thicknessPX.Length - 2);
                    if (int.TryParse(thicknessStr, out int thicknessInt))
                    {
                        if (thicknessInt <= 1)
                            thicknessList.Add(1);
                        else if (thicknessInt <= 3)
                            thicknessList.Add(3);
                        else if (thicknessInt <= 5 || thicknessInt > 5)
                            thicknessList.Add(5);
                        else
                            thicknessList.Add(1);
                    }

                    var color = borderParams[2];
                    colorList.Add(color);
                }
            }

            BorderWidth = (int)thicknessList.Average();

            if (colorList.Any())
            {
                var first = colorList.FirstOrDefault();
                if (colorList.All(x => x == first))
                    BorderColor = first;
            }
        }

        private void SetBorderWidthColor(string htmlBorder)
        {
            BorderWidth = 1;

            if (!string.IsNullOrEmpty(htmlBorder))
            {
                var borderParams = htmlBorder.Split(new char[] { ' ' });
                if (borderParams.Length == 3)
                {
                    var thicknessPX = borderParams[0];
                    var thicknessStr = thicknessPX.Substring(0, thicknessPX.Length - 2);
                    if (int.TryParse(thicknessStr, out int thickness))
                    {
                        if (thickness <= 1)
                            BorderWidth = 1;
                        else if (thickness <= 3)
                            BorderWidth = 3;
                        else if (thickness <= 5 || thickness > 5)
                            BorderWidth = 5;
                        else
                            BorderWidth = 1;
                    }

                    BorderColor = borderParams[2];
                }
            }
        }

        public void SetEditSettings(SheetCellEditSettings editSettings)
        {
            EditSettings.ControlKind = editSettings.ControlKind;
            EditSettings.NumberDigits = editSettings.NumberDigits;
            EditSettings.ItemsSource = editSettings.ItemsSource;
            EditSettings.CellDataType = editSettings.CellDataType;

            EditSettings.Required = editSettings.Required;
            EditSettings.DefaultValue = editSettings.DefaultValue;
            EditSettings.AutoGenerate = editSettings.AutoGenerate;
            EditSettings.AutoGenerateMask = editSettings.AutoGenerateMask;
            EditSettings.AutoClearMethod = editSettings.AutoClearMethod;
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

        public void SetFormatType(string format)
        {
            switch (format)
            {
                case "":
                case null:
                    FormatType = CellFormatTypes.None;
                    break;
                case CellDisplayFormatConsts.Integer:
                    FormatType = CellFormatTypes.Integer;
                    break;
                case CellDisplayFormatConsts.IntegerTwoDecimalPlaces:
                    FormatType = CellFormatTypes.IntegerTwoDecimalPlaces;
                    break;
                case CellDisplayFormatConsts.IntegerThreeDecimalPlaces:
                    FormatType = CellFormatTypes.IntegerThreeDecimalPlaces;
                    break;
                case CellDisplayFormatConsts.IntegerWithSpaces:
                    FormatType = CellFormatTypes.IntegerWithSpaces;
                    break;
                case CellDisplayFormatConsts.IntegerWithSpacesTwoDecimalPlaces:
                    FormatType = CellFormatTypes.IntegerWithSpacesTwoDecimalPlaces;
                    break;
                case CellDisplayFormatConsts.IntegerWithSpacesThreeDecimalPlaces:
                    FormatType = CellFormatTypes.IntegerWithSpacesThreeDecimalPlaces;
                    break;
                case CellDisplayFormatConsts.IntegerNegativeWithSpaces:
                    FormatType = CellFormatTypes.IntegerNegativeWithSpaces;
                    break;
                case CellDisplayFormatConsts.IntegerNegativeWithSpacesTwoDecimalPlaces:
                    FormatType = CellFormatTypes.IntegerNegativeWithSpacesTwoDecimalPlaces;
                    break;
                case CellDisplayFormatConsts.IntegerNegativeWithSpacesThreeDecimalPlaces:
                    FormatType = CellFormatTypes.IntegerNegativeWithSpacesThreeDecimalPlaces;
                    break;
                case CellDisplayFormatConsts.Date:
                    FormatType = CellFormatTypes.Date;
                    break;
                case CellDisplayFormatConsts.DateTime:
                    FormatType = CellFormatTypes.DateTime;
                    break;
                default:
                    FormatType = CellFormatTypes.Custom;
                    CustomFormat = format;
                    break;
            }
        }
    }
}