using System;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetRange
    {
        public string SheetName { get; set; } = string.Empty;
        public int RowStart { get; set; }
        public int RowEnd { get; set; }
        public int ColumnStart { get; set; }
        public int ColumnEnd { get; set; }

        public SheetRange()
        {
            
        }

        public SheetRange(int rowStart, int columnStart, int rowEnd, int columnEnd)
        {
            RowStart = rowStart;
            ColumnStart = columnStart;

            RowEnd = rowEnd;
            ColumnEnd = columnEnd;
        }

        public SheetRange(string rangeStr, int currentRow, int currentColumn)
        {
            Parse(rangeStr);

            if (RowStart <= 0)
            {
                RowStart += currentRow;
            }

            if (RowEnd <= 0)
            {
                RowEnd += currentRow;
            }

            if (ColumnStart <= 0)
            {
                ColumnStart += currentColumn;
            }

            if (ColumnStart <= 0)
            {
                ColumnStart += currentColumn;
            }
        }
        
        public bool IsCellInRange(int row, int column)
        {
            var isInRange = row >= RowStart 
                            && row <= RowEnd 
                            && column >= ColumnStart 
                            && column <= ColumnEnd;

            return isInRange;
        }

        private void Parse(string rangeStr)
        {
            
            var parts = rangeStr.Split(':');

            if (parts.Length > 0)
            {
                var startAddress = new SheetCellAddress(parts[0]);

                SheetName = startAddress.SheetName;
                RowStart = startAddress.Row;
                ColumnStart = startAddress.Column;
            }

            if (parts.Length > 1)
            {
                var endAddress = new SheetCellAddress(parts[1]);
                
                RowEnd = endAddress.Row;
                ColumnEnd = endAddress.Column;
            }
            else
            {
                RowEnd = RowStart;
                ColumnEnd = ColumnStart;
            }
        }
    }
}