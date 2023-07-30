using System;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetRange
    {
        private bool _rStartIsRelative;
        private bool _rEndIsRelative;
        private bool _cStartIsRelative;
        private bool _cEndIsRelative;
        public string SheetName { get; set; } = string.Empty;
        public int RowStart { get; set; }
        public int RowEnd { get; set; }
        public int ColumnStart { get; set; }
        public int ColumnEnd { get; set; }
        
        public bool RowStartIsRelative => _rStartIsRelative;
        public bool RowEndIsRelative => _rEndIsRelative;
        public bool ColumnStartIsRelative => _cStartIsRelative;
        public bool ColumnEndIsRelative => _cEndIsRelative;
        
        public SheetCellAddress StartAddress { get; set; }
        public SheetCellAddress EndAddress { get; set; }

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
            Parse(rangeStr, currentRow, currentColumn);

            if (RowStart < 0)
                RowStart += currentRow;

            if (RowEnd < 0)
                RowEnd += currentRow;

            if (ColumnStart < 0)
                ColumnStart += currentColumn;

            if (ColumnStart < 0)
                ColumnStart += currentColumn;
        }
        
        public bool IsCellInRange(int row, int column)
        {
            var isInRange = row >= RowStart 
                            && row <= RowEnd 
                            && column >= ColumnStart 
                            && column <= ColumnEnd;

            return isInRange;
        }

        private void Parse(string rangeStr, int currentRow, int currentColumn)
        {
            var parts = rangeStr.Split(':');
            if (parts.Length > 0)
            {
                StartAddress = new SheetCellAddress(parts[0], currentRow, currentColumn);

                _rStartIsRelative = StartAddress.RowIsRelative;
                _cStartIsRelative = StartAddress.ColumnIsRelative;

                SheetName = StartAddress.SheetName;
                RowStart = StartAddress.Row;
                ColumnStart = StartAddress.Column;
            }

            if (parts.Length > 1)
            {
                EndAddress = new SheetCellAddress(parts[1], currentRow, currentColumn);
                
                _rEndIsRelative = EndAddress.RowIsRelative;
                _cEndIsRelative = EndAddress.ColumnIsRelative;
                
                RowEnd = EndAddress.Row;
                ColumnEnd = EndAddress.Column;
            }
            else
            {
                RowEnd = RowStart;
                ColumnEnd = ColumnStart;
            }
        }
    }
}