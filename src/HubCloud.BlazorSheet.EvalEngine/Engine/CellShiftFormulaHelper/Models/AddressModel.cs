using System;
using System.Text;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Models
{
    public class AddressModel
    {
        public string SheetName { get; set; }
        public int RowValue { get; set; }
        public int ColumnValue { get; set; }
        
        /// <summary>
        /// R[1]C1 format
        /// </summary>
        public bool IsRowRelative { get; set; }
        /// <summary>
        /// R1C[1] format
        /// </summary>
        public bool IsColumnRelative { get; set; }
        /// <summary>
        /// RC1 format
        /// </summary>
        public bool IsRowCurrent { get; set; }
        /// <summary>
        /// R1C format
        /// </summary>
        public bool IsColumnCurrent { get; set; }

        public AddressModel(string address, SheetCellAddress cellAddress)
        {
            address = address.Trim();

            if (address.Contains("!"))
            {
                var separatorIndex = address.IndexOf('!');
                SheetName = address.Substring(0, separatorIndex);
                address = address.Substring(separatorIndex + 1);
            }

            var rIndex = address.IndexOf("R", StringComparison.OrdinalIgnoreCase);
            var cIndex = address.IndexOf("C", StringComparison.OrdinalIgnoreCase);

            var rowValue = address.Substring(1, cIndex - rIndex - 1);
            var columnValue = address.Substring(cIndex + 1);

            // row
            if (!string.IsNullOrEmpty(rowValue))
            {
                if (rowValue.Contains("[") && rowValue.Contains("["))
                    IsRowRelative = true;
                if (int.TryParse(rowValue.Trim(new[] {'[', ']'}), out var row))
                    RowValue = row;
            }
            else
            {
                IsRowCurrent = true;
                RowValue = cellAddress.Row;
            }
            
            // column
            if (!string.IsNullOrEmpty(columnValue))
            {
                if (columnValue.Contains("[") && columnValue.Contains("["))
                    IsColumnRelative = true;
                if (int.TryParse(columnValue.Trim(new[] {'[', ']'}), out var column))
                    ColumnValue = column;
            }
            else
            {
                IsColumnCurrent = true;
                ColumnValue = cellAddress.Column;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SheetName))
                sb.Append($"{SheetName}!");

            sb.Append("R");
            if (IsRowRelative)
                sb.Append($"[{RowValue}]");
            else if (!IsRowCurrent)
                sb.Append($"{RowValue}");
            
            sb.Append("C");
            if (IsColumnRelative)
                sb.Append($"[{ColumnValue}]");
            else if (!IsColumnCurrent)
                sb.Append($"{ColumnValue}");

            return sb.ToString();
        }
    }
}