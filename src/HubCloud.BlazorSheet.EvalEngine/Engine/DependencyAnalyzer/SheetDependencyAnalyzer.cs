using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.DependencyAnalyzer
{
    public class SheetDependencyAnalyzer
    {
        private readonly Sheet _sheet;
        public SheetDependencyAnalyzer(Sheet sheet)
        {
            _sheet = sheet;
        }
        
        public IEnumerable<SheetCell> GetDependencyCells(SheetCellAddress cellAddress)
        {
            var dependCells = new List<SheetCell>();
            foreach (var formulaCell in _sheet.Cells.Where(x => !string.IsNullOrEmpty(x.Formula)))
            {
                if (IsFormulaCellContainsAddress(formulaCell, cellAddress))
                    dependCells.Add(formulaCell);
            }

            return dependCells;
        }

        private bool IsFormulaCellContainsAddress(SheetCell formulaCell, SheetCellAddress currentCellAddress)
        {
            var formulaCellAddress = _sheet.CellAddress(formulaCell);
            
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

        #region public static methods
        public static string NormalizeAddress(string address, SheetCellAddress formulaCellAddress)
        {
            var rowVal = GetRowValue(address);
            var colVal = GetColValue(address);

            if (rowVal is null || colVal is null)
                return null;
            
            var normalizedAddress = new StringBuilder("R");
            
            // row
            if (rowVal == string.Empty)
                normalizedAddress.Append(formulaCellAddress.Row);
            else if (rowVal.Contains("-"))
            {
                var d = int.Parse(rowVal);
                normalizedAddress.Append(formulaCellAddress.Row + d);
            }
            else
                normalizedAddress.Append(rowVal);

            normalizedAddress.Append("C");
            
            // col
            if (colVal == string.Empty)
                normalizedAddress.Append(formulaCellAddress.Column);
            else if (colVal.Contains("-"))
            {
                var d = int.Parse(colVal);
                normalizedAddress.Append(formulaCellAddress.Column + d);
            }
            else
                normalizedAddress.Append(colVal);

            return normalizedAddress.ToString();
        }

        private static string GetRowValue(string address)
        {
            if (string.IsNullOrEmpty(address))
                return null;
            
            address = address.ToUpper();
            var index = address.IndexOf("R", StringComparison.InvariantCulture);
            if (index == -1)
                return null;

            index++;
            var sb = new StringBuilder();
            while (address[index] != 'C')
            {
                sb.Append(address[index]);
                index++;
            }

            return sb.ToString();
        }
        
        private static string GetColValue(string address)
        {
            if (string.IsNullOrEmpty(address))
                return null;
            
            address = address.ToUpper();
            var index = address.IndexOf("C", StringComparison.InvariantCulture);
            if (index == -1)
                return null;

            index++;
            var sb = new StringBuilder();
            while (index < address.Length)
            {
                sb.Append(address[index]);
                index++;
            }

            return sb.ToString();
        }

        public static string GetCellAddress(SheetCellAddress cellAddress)
            => $"R{cellAddress.Row}C{cellAddress.Column}";
        #endregion
    }
}