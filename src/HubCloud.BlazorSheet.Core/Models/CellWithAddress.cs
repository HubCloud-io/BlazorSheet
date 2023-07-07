using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class CellWithAddress
    {
        public SheetCell Cell { get; set; }
        public SheetCellAddress Address { get; set; }
    }
}
