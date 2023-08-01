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
        public bool IsRowRelative { get; set; }
        public bool IsColumnRelative { get; set; }

        public AddressModel(string address)
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

            if (rowValue.Contains("[") && rowValue.Contains("["))
                IsRowRelative = true;
            if (columnValue.Contains("[") && columnValue.Contains("["))
                IsColumnRelative = true;

            if (int.TryParse(rowValue.Trim(new[] {'[', ']'}), out var row))
                RowValue = row;
            if (int.TryParse(columnValue.Trim(new[] {'[', ']'}), out var column))
                ColumnValue = column;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SheetName))
                sb.Append($"{SheetName}!");

            sb.Append("R");
            if (IsRowRelative)
                sb.Append($"[{RowValue}]");
            else
                sb.Append($"{RowValue}");
            
            sb.Append("C");
            if (IsColumnRelative)
                sb.Append($"[{ColumnValue}]");
            else
                sb.Append($"{ColumnValue}");

            return sb.ToString();
        }
    }
}