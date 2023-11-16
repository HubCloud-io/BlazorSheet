using System;

namespace HubCloud.BlazorSheet.Core.Models
{
    public struct ValueAddress
    {
        private bool _isRowRelative;
        private bool _isColumnRelative;
        
        public int Row { get; set; }
        public int Column { get; set; }
        public string SheetName { get; set; }

        public ValueAddress(int row, int column, string sheetName = null)
        {
            _isRowRelative = false;
            _isColumnRelative = false;
            
            Row = row;
            Column = column;
            SheetName = sheetName ?? string.Empty;
        }
        
        public ValueAddress(string address)
        {
            _isRowRelative = false;
            _isColumnRelative = false;
            
            Row = 0;
            Column = 0;
            SheetName = string.Empty;
            Parse(address);
        }

        public ValueAddress(string address, int currentRow, int currentColumn)
        {
            _isRowRelative = false;
            _isColumnRelative = false;
            
            Row = 0;
            Column = 0;
            SheetName = string.Empty;
            
            Parse(address);

            if (Row == 0 || _isRowRelative)
                Row += currentRow;

            if (Column == 0 || _isColumnRelative)
                Column += currentColumn;
        }
        
        public ValueAddress(SheetCellAddress sheetCellAddress)
        {
            _isRowRelative = false;
            _isColumnRelative = false;
            
            Row = sheetCellAddress.Row;
            Column = sheetCellAddress.Column;
            SheetName = sheetCellAddress.SheetName;
        }
        
        private void Parse(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address can't be null or empty.", nameof(address));

            address = address.Replace(" ", "");
            
            if (address.Contains("!"))
            {
                var separatorIndex = address.IndexOf('!');
                SheetName = address.Substring(0, separatorIndex);
                address = address.Substring(separatorIndex + 1);
            }

            var rIndex = address.IndexOf("R", StringComparison.OrdinalIgnoreCase);
            var cIndex = address.IndexOf("C", StringComparison.OrdinalIgnoreCase);

            if (rIndex < 0 || cIndex < 0 || cIndex < rIndex)
                throw new ArgumentException("Invalid cell address format.", nameof(address));

            var rString = address.Substring(1, cIndex - rIndex - 1);
            var cString = address.Substring(cIndex + 1);

            if (rString.Contains("[") && rString.Contains("]"))
                rString = rString.Replace("[", "")
                    .Replace("]", "");

            if (cString.Contains("[") && cString.Contains("]"))
                cString = cString.Replace("[", "")
                    .Replace("]", "");

            int.TryParse(rString, out var row);
            Row = row;

            int.TryParse(cString, out var col);
            Column = col;
        }
    }
}