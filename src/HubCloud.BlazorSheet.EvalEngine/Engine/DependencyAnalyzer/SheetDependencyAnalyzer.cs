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
        private List<string> _processedCells;

        public SheetDependencyAnalyzer(Sheet sheet)
        {
            _sheet = sheet;
        }

        public IEnumerable<SheetCell> GetDependencyCells(SheetCellAddress cellAddress)
        {
            _processedCells = new List<string>(GetNoFormulaCells(_sheet));

            var dependCellsDict = GetDependencyCellsInner(cellAddress);

            var notDependedFormulas = GetNotDependedFormulaCells(_sheet, dependCellsDict);
            _processedCells.AddRange(notDependedFormulas);

            var orderedCells = OrderCellsForCalc(_processedCells, dependCellsDict);
            return orderedCells;
        }

        private IEnumerable<string> GetNotDependedFormulaCells(Sheet sheet, Dictionary<string, SheetCell> dependCellsDict)
            => sheet.Cells
                .Where(x => !string.IsNullOrEmpty(x.Formula))
                .Select(x => GetCellAddress(sheet.CellAddress(x)))
                .Where(x => !dependCellsDict.Select(s => s.Key).Contains(x));


        private Dictionary<string, SheetCell> GetDependencyCellsInner(SheetCellAddress cellAddress)
        {
            var currentCellAddress = GetCellAddress(cellAddress);
            var formulaCells = _sheet.Cells
                .Where(x => !string.IsNullOrEmpty(x.Formula))
                .ToList();

            var dependCellsDict = new Dictionary<string, SheetCell>();
            foreach (var formulaCell in formulaCells)
            {
                if (IsFormulaCellContainsAddress(formulaCell, currentCellAddress))
                {
                    var address = GetCellAddress(_sheet.CellAddress(formulaCell));
                    dependCellsDict.Add(address, formulaCell);
                }
            }

            // if (!_processedCells.Contains(currentCellAddress.ToUpper()))
            //     _processedCells.Add(currentCellAddress.ToUpper());
            // foreach (var item in dependCellsDict.Select(x => x.Key))
            // {
            //     if (!_processedCells.Contains(item))
            //         _processedCells.Add(item);
            // }

            foreach (var cell in dependCellsDict.ToArray())
            {
                var address = _sheet.CellAddress(cell.Value);
                var dependencyCells = GetDependencyCellsInner(address);
                foreach (var dependencyCell in dependencyCells)
                {
                    var strAddress = GetCellAddress(_sheet.CellAddress(dependencyCell.Value));
                    if (!dependCellsDict.ContainsKey(strAddress))
                        dependCellsDict.Add(strAddress, dependencyCell.Value);
                }
            }

            return dependCellsDict;
        }

        public List<SheetCell> OrderCellsForCalc(List<string> processedCells,
            Dictionary<string, SheetCell> dependCellsDict)
        {
            var list = new List<SheetCell>();
            while (dependCellsDict.Count != 0)
            {
                var canCalc = false;
                foreach (var dependCell in dependCellsDict.ToArray())
                {
                    if (CanCalc(dependCell.Value, processedCells))
                    {
                        var normalizedAddress = NormalizeAddress(dependCell.Key, _sheet.CellAddress(dependCell.Value));
                        if (!processedCells.Contains(normalizedAddress))
                            processedCells.Add(normalizedAddress);

                        canCalc = true;

                        list.Add(dependCell.Value);
                        dependCellsDict.Remove(dependCell.Key);
                    }
                }

                if (!canCalc)
                    break;
            }

            return list;
        }

        private bool CanCalc(SheetCell formulaCell, List<string> processedCells)
        {
            var formulaCellAddress = _sheet.CellAddress(formulaCell);
            var formula = formulaCell.Formula.ToUpper();

            // check address ranges
            var rangeRegex = RegexHelper.AddressRangeRegex;
            var rangeMatches = rangeRegex.Matches(formula)
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToArray();

            foreach (var rangeMatch in rangeMatches)
            {
                var rangeAddresses = GetAddressListByRange(rangeMatch);
                foreach (var rangeAddress in rangeAddresses)
                {
                    if (!processedCells.Contains(rangeAddress))
                        return false;
                }
            }

            // check simple addresses
            var addressRegex = RegexHelper.AddressRegex;
            var addressMatches = addressRegex.Matches(formula)
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToArray();

            foreach (var match in addressMatches)
            {
                var normalizerAddress = NormalizeAddress(match.ToUpper(), formulaCellAddress);
                if (!processedCells.Contains(normalizerAddress))
                    return false;
            }

            return true;
        }

        private bool IsFormulaCellContainsAddress(SheetCell formulaCell, string cellAddress)
        {
            var formulaCellAddress = _sheet.CellAddress(formulaCell);

            var sb = new StringBuilder(formulaCell.Formula);

            // check address ranges
            var rangeRegex = RegexHelper.AddressRangeRegex;
            var rangeMatches = rangeRegex.Matches(sb.ToString())
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToArray();

            foreach (var range in rangeMatches)
            {
                if (IsAddressInRange(cellAddress, range))
                    return true;

                sb.Replace(range, "{R}");
            }

            // check simple addresses
            var addressRegex = RegexHelper.AddressRegex;
            var addressMatches = addressRegex.Matches(sb.ToString())
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToArray();

            foreach (var match in addressMatches)
            {
                if (NormalizeAddress(match.ToUpper(), formulaCellAddress) == cellAddress)
                    return true;
            }

            return false;
        }


        #region public static methods

        public static List<string> GetNoFormulaCells(Sheet sheet)
            => sheet.Cells
                .Where(x => string.IsNullOrEmpty(x.Formula))
                .Select(x => GetCellAddress(sheet.CellAddress(x)))
                .ToList();


        public static List<string> GetAddressListByRange(string range)
        {
            var list = new List<string>();
            var arr = range.Split(':').ToArray();
            if (arr.Length != 2)
                return list;

            var startAddress = arr[0];
            var endAddress = arr[1];

            var startRow = int.Parse(GetRowValue(startAddress));
            var startCol = int.Parse(GetColValue(startAddress));

            var endRow = int.Parse(GetRowValue(endAddress));
            var endCol = int.Parse(GetColValue(endAddress));

            for (var r = startRow; r <= endRow; r++)
            {
                for (var c = startCol; c <= endCol; c++)
                {
                    list.Add($"R{r}C{c}");
                }
            }

            return list;
        }

        public static bool IsAddressInRange(string cellAddress, string addressRange)
        {
            var arr = addressRange.Split(':').ToArray();
            if (arr.Length != 2)
                return false;

            var startAddress = arr[0];
            var endAddress = arr[1];

            var startRow = int.Parse(GetRowValue(startAddress));
            var startCol = int.Parse(GetColValue(startAddress));

            var endRow = int.Parse(GetRowValue(endAddress));
            var endCol = int.Parse(GetColValue(endAddress));

            var currentAddressRow = int.Parse(GetRowValue(cellAddress));
            var currentAddressCol = int.Parse(GetColValue(cellAddress));

            if (startRow <= currentAddressRow && endRow >= currentAddressRow &&
                startCol <= currentAddressCol && endCol >= currentAddressCol)
                return true;

            return false;
        }

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