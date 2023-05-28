using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class HbSheetColumn
    {
        public int Number { get; set; }
        public double WidthValue { get; set; }
        public string Width => (WidthValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)) + "px";

        public HbSheetColumn()
        {

        }

        public HbSheetColumn(int number, double widthValue)
        {
            Number = number;
            widthValue = widthValue;
        }

    }
}
