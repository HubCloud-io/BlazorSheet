using System;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.DependencyAnalyzer
{
    public struct ValueAddress
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public ValueAddress(int row, int column)
        {
            Row = row;
            Column = column;
        }
        
        public ValueAddress(SheetCellAddress sheetCellAddress)
        {
            Row = sheetCellAddress.Row;
            Column = sheetCellAddress.Column;
        }

        public ValueAddress(string address)
        {
            Row = 0;
            Column = 0;
            Parse(address);
        }
        
        private void Parse(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address can't be null or empty.", nameof(address));

            address = address.Replace(" ", "");
            
            if (address.Contains("!"))
            {
                var separatorIndex = address.IndexOf('!');
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