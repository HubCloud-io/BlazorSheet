using System;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetCellAddress
    {
        public string SheetName { get; set; } = string.Empty;
        public int Row { get; set; }
        public int Column { get; set; }

        public SheetCellAddress()
        {

        }

        public SheetCellAddress(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public SheetCellAddress(string address)
        {
            Parse(address);
        }
        
        public SheetCellAddress(string address, int currentRow, int currentColumn)
        {
            Parse(address);

            if (Row < 0)
            {
                Row += currentRow;
            }

            if (Column < 0)
            {
                Column += currentColumn;
            }
        }
        
        
        private void Parse(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("Address can't be null or empty.", nameof(address));
            }

            address = address.Replace(" ", "");

            if (address.Contains("!"))
            {
                var separatorIndex = address.IndexOf('!');
                SheetName = address.Substring(0, separatorIndex);
                address = address.Substring(separatorIndex+1);
            }
            
            int rIndex = address.IndexOf("R", StringComparison.OrdinalIgnoreCase);
            int cIndex = address.IndexOf("C", StringComparison.OrdinalIgnoreCase);

            if (rIndex < 0 || cIndex < 0 || cIndex < rIndex)
            {
                throw new ArgumentException("Invalid cell address format.", nameof(address));
            }
            
            var rString = address.Substring(1, cIndex - rIndex - 1);
            var cString = address.Substring(cIndex + 1);

            if (!int.TryParse(rString, out int row) ||
                !int.TryParse(cString, out int col))
            {
                throw new ArgumentException("Invalid cell address format.", nameof(address));
            }

            Row = row;
            Column = col;
        }

    }
}