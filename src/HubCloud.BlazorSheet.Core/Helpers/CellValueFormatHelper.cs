using HubCloud.BlazorSheet.Core.Consts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Helpers
{
    public static class CellValueFormatHelper
    {
        public static string ToStringWithFormat(decimal value, string format)
        {
            var result = string.Empty;

            switch (format)
            {
                case CellFormatConsts.Integer:
                    result = value.ToString("##0", CultureInfo.InvariantCulture);
                    break;
                case CellFormatConsts.IntegerTwoDecimalPlaces:
                    result = value.ToString("##0.00", new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberDecimalSeparator = "." } });
                    break;
                case CellFormatConsts.IntegerThreeDecimalPlaces:
                    result = value.ToString("##0.000", new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberDecimalSeparator = "." } });
                    break;
                case CellFormatConsts.IntegerWithSpaces:
                    result = value.ToString("#,##0", new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " " } });
                    break;
                case CellFormatConsts.IntegerWithSpacesTwoDecimalPlaces:
                    result = value.ToString("#,##0.00", new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalSeparator = "." } });
                    break;
                case CellFormatConsts.IntegerWithSpacesThreeDecimalPlaces:
                    result = value.ToString("#,##0.000", new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalSeparator = "." } });
                    break;
                case CellFormatConsts.IntegerNegativeWithSpaces:
                    result = value.ToString("0:#,##0;(#,##0)", new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " " } });
                    break;
                case CellFormatConsts.IntegerNegativeWithSpacesTwoDecimalPlaces:
                    result = value.ToString("0:#,##0.00;(#,##0.00)", new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalSeparator = "." } });
                    break;
                case CellFormatConsts.IntegerNegativeWithSpacesThreeDecimalPlaces:
                    result = value.ToString("0:#,##0.000;(#,##0.000)", new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalSeparator = "." } });
                    break;
                case CellFormatConsts.None:
                    result = value.ToString(new CultureInfo(127) { NumberFormat = new NumberFormatInfo { NumberDecimalSeparator = "." } });
                    break;
                default:
                    result = value.ToString(format, CultureInfo.InvariantCulture);
                    break;
            }

            return result;
        }
    }
}
