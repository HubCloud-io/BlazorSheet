using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class HbSheet
    {
        public int RowsCount { get; set; }
        public int ColumnsCount { get; set; }

        public List<HbSheetRow> Rows { get; set; } = new List<HbSheetRow>();
        public List<HbSheetColumn> Columns { get; set; } = new List<HbSheetColumn>();

        public List<HbSheetCell> Cells { get; set; } = new List<HbSheetCell>();
    }
}
