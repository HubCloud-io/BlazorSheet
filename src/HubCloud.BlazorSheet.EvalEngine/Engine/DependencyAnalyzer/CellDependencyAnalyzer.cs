using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.DependencyAnalyzer
{
    public class CellDependencyAnalyzer
    {
        public IEnumerable<SheetCell> GetDependencyCells(SheetCellAddress cellAddress, Sheet sheet)
        {
            var dependCells = new List<SheetCell>();
            foreach (var cell in sheet.Cells.Where(x => !string.IsNullOrEmpty(x.Formula)))
            {
                if (IsContainsCurrentAddress(cell, sheet.CellAddress(cell), cellAddress))
                    dependCells.Add(cell);
            }

            return dependCells;
        }

        private bool IsContainsCurrentAddress(SheetCell formulaCell,
            SheetCellAddress formulaCellAddress,
            SheetCellAddress currentCellAddress)
        {
            var regex = new Regex(@"R-*\d*C-*\d*");
            var matches = regex.Matches(formulaCell.Formula)
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToArray();
            
            foreach (var match in matches)
            {
                if (NormalizeAddress(match.ToUpper(), formulaCellAddress) == GetCellAddress(currentCellAddress))
                    return true;
            }

            return false;
        }

        private string NormalizeAddress(string address, SheetCellAddress formulaCellAddress)
        {
            if (!address.Contains("-"))
                return address;

            var normalizedAddress = new StringBuilder("R");
            
            var arr = address.ToUpper()
                .Trim()
                .Replace("R", "")
                .Replace("C", ".")
                .Split('.');

            if (decimal.TryParse(arr[0], out var row) && row < 0)
                normalizedAddress.Append(formulaCellAddress.Row - 1);
            else
                normalizedAddress.Append(arr[0]);

            normalizedAddress.Append("C");
            
            if (decimal.TryParse(arr[1], out var col) && col < 0)
                normalizedAddress.Append(formulaCellAddress.Column - 1);
            else
                normalizedAddress.Append(arr[1]);
                
            return normalizedAddress.ToString();
        }

        private string GetCellAddress(SheetCellAddress cellAddress)
            => $"R{cellAddress.Row}C{cellAddress.Column}";
    }
}