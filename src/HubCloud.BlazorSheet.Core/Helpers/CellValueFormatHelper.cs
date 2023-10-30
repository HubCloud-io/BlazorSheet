using HubCloud.BlazorSheet.Core.Consts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace HubCloud.BlazorSheet.Core.Helpers
{
    public static class CellValueFormatHelper
    {
        public static string ToStringWithFormat(decimal value, string cellFormat)
        {
            var result = string.Empty;

            switch (cellFormat)
            {
                case CellDisplayFormatConsts.Integer:
                    result = value.ToString(CellToStringFormatConsts.Integer, CultureInfo.InvariantCulture);
                    break;
                case CellDisplayFormatConsts.IntegerTwoDecimalPlaces:
                    result = value.ToString(CellToStringFormatConsts.IntegerTwoDecimalPlaces, new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberDecimalSeparator = "." } });
                    break;
                case CellDisplayFormatConsts.IntegerThreeDecimalPlaces:
                    result = value.ToString(CellToStringFormatConsts.IntegerThreeDecimalPlaces, new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberDecimalSeparator = "." } });
                    break;
                case CellDisplayFormatConsts.IntegerWithSpaces:
                    result = value.ToString(CellToStringFormatConsts.IntegerWithSpaces, new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " " } });
                    break;
                case CellDisplayFormatConsts.IntegerWithSpacesTwoDecimalPlaces:
                    result = value.ToString(CellToStringFormatConsts.IntegerWithSpacesTwoDecimalPlaces, new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalSeparator = "." } });
                    break;
                case CellDisplayFormatConsts.IntegerWithSpacesThreeDecimalPlaces:
                    result = value.ToString(CellToStringFormatConsts.IntegerWithSpacesThreeDecimalPlaces, new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalSeparator = "." } });
                    break;
                case CellDisplayFormatConsts.IntegerNegativeWithSpaces:
                    var format = value < 0 ? CellToStringFormatConsts.IntegerNegativeWithSpaces : CellToStringFormatConsts.IntegerWithSpaces;
                    result = value.ToString(format, new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " " } });
                    break;
                case CellDisplayFormatConsts.IntegerNegativeWithSpacesTwoDecimalPlaces:
                    format = value < 0 ? CellToStringFormatConsts.IntegerNegativeWithSpacesTwoDecimalPlaces : CellToStringFormatConsts.IntegerWithSpacesTwoDecimalPlaces;
                    result = value.ToString(format, new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalSeparator = "." } });
                    break;
                case CellDisplayFormatConsts.IntegerNegativeWithSpacesThreeDecimalPlaces:
                    format = value < 0 ? CellToStringFormatConsts.IntegerNegativeWithSpacesThreeDecimalPlaces : CellToStringFormatConsts.IntegerWithSpacesThreeDecimalPlaces;
                    result = value.ToString(format, new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalSeparator = "." } });
                    break;
                case CellDisplayFormatConsts.None:
                    result = value.ToString(new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberDecimalSeparator = "." } });
                    break;
                default:
                    result = value.ToString(cellFormat, CultureInfo.InvariantCulture);
                    break;
            }

            return result;
        }

        public static string ToStringWithFormat(string value, string cellFormat)
        {
            var result = string.Empty;

            if (decimal.TryParse(
                            value.Replace(',', '.'),
                            NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                            new NumberFormatInfo { NumberDecimalSeparator = "." },
                            out var decimalValue))
                result = CellValueFormatHelper.ToStringWithFormat(decimalValue, cellFormat);
            else if (DateTime.TryParse(value, out var dateTimeValue))
                result = dateTimeValue.ToString(cellFormat);
            else
                result = value;

            return result;
        }
    }
}
