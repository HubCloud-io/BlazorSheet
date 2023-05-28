using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class HbSheetCell
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public string BackgroundColor { get; set; }
        public string Color { get; set; }
        public string FontWeight { get; set; }
        public string FontSize { get; set; }
        public string FontFamily { get; set; }

        public string BorderLeft { get; set; }
        public string BorderRight { get; set; }
        public string BorderTop { get; set; }
        public string BorderBottom { get; set; }

        public string TextAlign { get; set; }
        public int ColSpan { get; set; }

        public string Text { get; set; }
        public object Value { get; set; }


    }
}
