using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class HbSheetRow
    {
        public int Number { get; set; }
        public int ParentNumber { get; set; }
        public double HeightValue { get; set; }

        public string Height => (HeightValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)) + "px";

        public HbSheetRow()
        {

        }

        public HbSheetRow(int number, double heightValue)
        {
            Number = number;
            HeightValue = heightValue;
        }

    }
}
