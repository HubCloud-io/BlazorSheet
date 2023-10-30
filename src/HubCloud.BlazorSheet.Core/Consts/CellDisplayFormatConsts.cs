using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Consts
{
    public static class CellDisplayFormatConsts
    {
        public const string None = "";

        public const string Integer = "100000";
        public const string IntegerTwoDecimalPlaces = "100000.23";
        public const string IntegerThreeDecimalPlaces = "100000.234";

        public const string IntegerWithSpaces = "100 000";
        public const string IntegerWithSpacesTwoDecimalPlaces = "100 000.23";
        public const string IntegerWithSpacesThreeDecimalPlaces = "100 000.234";

        public const string IntegerNegativeWithSpaces = "(100 000)";
        public const string IntegerNegativeWithSpacesTwoDecimalPlaces = "(100 000.23)";
        public const string IntegerNegativeWithSpacesThreeDecimalPlaces = "(100 000.234)";

        public const string Date = "dd.MM.yyyy";
        public const string DateTime = "dd.MM.yyyy HH:mm:ss";
    }
}
