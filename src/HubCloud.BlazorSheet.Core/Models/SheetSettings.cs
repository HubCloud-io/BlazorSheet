using System.Collections.Generic;
using System.Linq;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetSettings
    {
        public int RowsCount { get; set; }
        public int ColumnsCount { get; set; }
        
        public List<SheetRow> Rows { get; set; } = new List<SheetRow>();
        public List<SheetColumn> Columns { get; set; } = new List<SheetColumn>();

        public List<SheetCell> Cells { get; set; } = new List<SheetCell>();
        public List<SheetCellStyle> Styles { get; set; } = new List<SheetCellStyle>();

        public void SetStyles(List<SheetCellStyle> styles)
        {
            foreach (var styleItem in styles)
            {
                if (Cells.All(x => x.StyleUid != styleItem.Uid))
                {
                    continue;
                }
                
                Styles.Add(styleItem);
            }
        }
    }
}