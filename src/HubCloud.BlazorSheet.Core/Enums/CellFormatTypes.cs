using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Enums
{
    public enum CellFormatTypes
    {
        None = 0,
        Integer = 1,
        IntegerTwoDecimalPlaces = 2,
        IntegerThreeDecimalPlaces = 3,
        Date = 4,
        DateTime = 5,
        Custom = 6,
        IntegerWithSpaces = 7,
        IntegerWithSpacesTwoDecimalPlaces = 8,
        IntegerWithSpacesThreeDecimalPlaces = 9,
        IntegerNegativeWithSpaces = 10,
        IntegerNegativeWithSpacesTwoDecimalPlaces = 11,
        IntegerNegativeWithSpacesThreeDecimalPlaces = 12
    }
}
