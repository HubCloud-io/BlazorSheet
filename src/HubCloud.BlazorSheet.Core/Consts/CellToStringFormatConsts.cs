using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Consts
{
    public class CellToStringFormatConsts
    {
        public const string None = "";

        public const string Integer = "##0";
        public const string IntegerTwoDecimalPlaces = "##0.00";
        public const string IntegerThreeDecimalPlaces = "##0.000";

        public const string IntegerWithSpaces = "#,##0";
        public const string IntegerWithSpacesTwoDecimalPlaces = "#,##0.00";
        public const string IntegerWithSpacesThreeDecimalPlaces = "#,##0.000";

        public const string IntegerNegativeWithSpaces = "0:#,##0;(#,##0)";
        public const string IntegerNegativeWithSpacesTwoDecimalPlaces = "0:#,##0.00;(#,##0.00)";
        public const string IntegerNegativeWithSpacesThreeDecimalPlaces = "0:#,##0.00;(#,##0.000)";
    }
}
