﻿using System;

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

        private bool _rIsRelative;
        private bool _cIsRelative;
        public SheetCellAddress(string address, int currentRow, int currentColumn)
        {
            Parse(address);
            
            if (Row == 0 || _rIsRelative)
                Row += currentRow;
            
            if (Column == 0 || _cIsRelative)
                Column += currentColumn;
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

            _rIsRelative = false;
            _cIsRelative = false;

            if (rString.Contains("[") && rString.Contains("]"))
            {
                _rIsRelative = true;
                rString = rString.Trim('[', ']');
            }

            if (cString.Contains("[") && cString.Contains("]"))
            {
                _cIsRelative = true;
                cString = cString.Trim('[', ']');
            }

            if (int.TryParse(rString, out var row))
                Row = row;

            if (int.TryParse(cString, out var col))
                Column = col;

            Row = row;
            Column = col;
        }

        public override string ToString() => "{SheetName}!R{Row}C{Column}";
    }
}