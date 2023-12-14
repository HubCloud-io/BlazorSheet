using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Models;
using HubCloud.BlazorSheet.EvalEngine.Helpers;
using Newtonsoft.Json;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.DependencyAnalyzer
{
    public class SheetDependencyAnalyzer
    {
        private readonly Sheet _sheet;

        // key - cell to change
        // value - cells with formula, that will be changed after the key cell is changed (with recursion depth 1)
        private Dictionary<ValueAddress, List<SheetCell>> _dependencyFormulaDict;
        // key - formula address
        // value - addresses in formula list
        private Dictionary<ValueAddress, List<ValueAddress>> _formulaAddressesDict;
        
        private readonly List<SheetCell> _formulaCells;

        public SheetDependencyAnalyzer(Sheet sheet)
        {
            _sheet = sheet;
            _formulaCells = sheet.Cells
                .Where(x => !string.IsNullOrEmpty(x.Formula))
                .ToList();

            // var sw = new Stopwatch();
            // sw.Start();
            InitDependencyDicts(_formulaCells);
            // sw.Stop();
            // var el = sw.Elapsed;
        }


        [Obsolete]
        public List<SheetCell> GetDependencyCells(SheetCellAddress cellAddress)
        {
            var processedCells = new List<string>(GetNoFormulaCells(_sheet));

            var dependCellsDict = GetDependencyCellsInner(cellAddress);

            var notDependedFormulas = GetNotDependedFormulaCells(_sheet, dependCellsDict);
            processedCells.AddRange(notDependedFormulas);

            var orderedCells = OrderCellsForCalc(processedCells, dependCellsDict);
            return orderedCells;
        }

        [Obsolete]
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

        public List<SheetCell> GetDependencyCells2(SheetCellAddress cellAddress)
        {
            var resultList = new List<SheetCell>();
            _dependencyFormulaDict.TryGetValue(new ValueAddress(cellAddress), out var dependList);

            if (dependList?.Any() == true)
            {
                resultList.AddRange(dependList);
                var innerCells = GetDependencyCells2Inner(dependList);
                if (innerCells?.Any() == true)
                    resultList.AddRange(innerCells);
            }

            var distList = new List<SheetCell>();
            foreach (var item in resultList)
            {
                if (!distList.Any(x => x.Uid == item.Uid))
                    distList.Add(item);
            }

            return distList;
        }


        public List<SheetCell> OrderCellsForCalc2()
        {
            var allFormulaCells = _formulaCells.ToList();
            var dependHashSet = new HashSet<ValueAddress>(_dependencyFormulaDict.Select(x => x.Key));

            var resultList = new List<SheetCell>();
            var iterationCount = 0;
            var dependHashSetCount = dependHashSet.Count;
            while (dependHashSet.Any())
            {
                iterationCount++;
                if (iterationCount > dependHashSetCount)
                {
                    var circularCells = GetCircularCellsStr(allFormulaCells);
                    throw new EvaluateException($"Formula circular dependency problem in cells: {circularCells}");
                }

                var toRemoveFromHasSet = new List<ValueAddress>();
                foreach (var formulaCell in allFormulaCells.ToArray())
                {
                    var formulaCellAddress = _sheet.CellAddressSlim(formulaCell);
                    if (!dependHashSet.Contains(formulaCellAddress))
                    {
                        resultList.Add(formulaCell);
                        allFormulaCells.Remove(formulaCell);
                        var addresses = _formulaAddressesDict[formulaCellAddress];
                        toRemoveFromHasSet.AddRange(addresses);
                    }
                }

                foreach (var item in toRemoveFromHasSet)
                {
                    dependHashSet.Remove(item);
                }
            }
            
            resultList.AddRange(allFormulaCells);
            resultList.Reverse();

            return resultList;
        }

        private string GetCircularCellsStr(List<SheetCell> badCells)
        {
            var resultStr = string.Empty;
            foreach (var badCell in badCells)
            {
                var badCellAddress = _sheet.CellAddressSlim(badCell);
                if (!_formulaAddressesDict.TryGetValue(badCellAddress, out var addressesInFormula))
                    continue;

                if (addressesInFormula?.Contains(badCellAddress) == true)
                    resultStr += $"R{badCellAddress.Row}C{badCellAddress.Column}; ";
            }
            
            return resultStr;
        }


        #region private methods

        private List<SheetCell> GetDependencyCells2Inner(List<SheetCell> cells)
        {
            var resultList = new List<SheetCell>();
            foreach (var cell in cells)
            {
                _dependencyFormulaDict.TryGetValue(new ValueAddress(_sheet.CellAddress(cell)), out var list);
                if (list?.Any() == true)
                {
                    resultList.AddRange(list);

                    var innerCells = GetDependencyCells2Inner(list);
                    if (innerCells?.Any() == true)
                        resultList.AddRange(innerCells);
                }
            }

            return resultList;
        }

        

        private void InitDependencyDicts(List<SheetCell> formulaCells)
        {
            _formulaAddressesDict = new Dictionary<ValueAddress, List<ValueAddress>>();
            _dependencyFormulaDict = new Dictionary<ValueAddress, List<SheetCell>>();
            
            foreach (var formulaCell in formulaCells)
            {
                var addressesInFormula = GetAllFormulaAddresses(formulaCell);
                var formulaCellAddress = _sheet.CellAddressSlim(formulaCell);
                if (!_formulaAddressesDict.ContainsKey(formulaCellAddress))
                    _formulaAddressesDict.Add(formulaCellAddress, addressesInFormula);
                else
                    _formulaAddressesDict[formulaCellAddress].AddRange(addressesInFormula);

                foreach (var address in addressesInFormula)
                {
                    if (!_dependencyFormulaDict.ContainsKey(address))
                    {
                        var list = new List<SheetCell>()
                        {
                            formulaCell
                        };
                        _dependencyFormulaDict.Add(address, list);
                    }
                    else
                    {
                        _dependencyFormulaDict[address].Add(formulaCell);
                    }
                }
            }
        }

        private List<ValueAddress> GetAllFormulaAddresses(SheetCell formulaCell)
        {
            var formulaCellAddress = _sheet.CellAddressSlim(formulaCell);

            // check address ranges
            var rangeRegex = RegexHelper.AddressRangeRegex;
            var rangeMatches = rangeRegex.Matches(formulaCell.Formula)
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToArray();

            var tmpList = new List<ValueAddress>();
            foreach (var match in rangeMatches)
            {
                var arr = match.Split(':');
                if (arr.Length != 2)
                    continue;

                var start = new ValueAddress(NormalizeAddress(arr[0], formulaCellAddress));
                var end = new ValueAddress(NormalizeAddress(arr[1], formulaCellAddress));

                for (var r = start.Row; r <= end.Row; r++)
                {
                    for (var c = start.Column; c <= end.Column; c++)
                    {
                        tmpList.Add(new ValueAddress(r, c));
                    }
                }
            }

            var addressDict = tmpList.Distinct().ToDictionary(x => x, x => x);

            // check simple addresses
            var addressRegex = RegexHelper.AddressRegex;
            var addressMatches = addressRegex.Matches(formulaCell.Formula)
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToArray();

            foreach (var match in addressMatches)
            {
                var addr = new ValueAddress(NormalizeAddress(match, formulaCellAddress));
                if (!addressDict.ContainsKey(addr))
                    addressDict.Add(addr, addr);
            }

            return addressDict.Select(x => x.Value).ToList();
        }

        private IEnumerable<string> GetNotDependedFormulaCells(Sheet sheet,
            Dictionary<string, SheetCell> dependCellsDict)
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
                var rangeAddresses = GetAddressListByRange(rangeMatch, formulaCellAddress);
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
                if (IsAddressInRange(cellAddress, range, formulaCellAddress))
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

        #endregion

        #region public static methods

        public static List<string> GetNoFormulaCells(Sheet sheet)
            => sheet.Cells
                .Where(x => string.IsNullOrEmpty(x.Formula))
                .Select(x => GetCellAddress(sheet.CellAddress(x)))
                .ToList();


        public static List<string> GetAddressListByRange(string range, SheetCellAddress cellAddress)
        {
            var list = new List<string>();
            var arr = range.Split(':').ToArray();
            if (arr.Length != 2)
                return list;

            var startAddress = arr[0];
            var endAddress = arr[1];

            var startRow = int.Parse(GetRowValue(startAddress, cellAddress));
            var startCol = int.Parse(GetColValue(startAddress, cellAddress));

            var endRow = int.Parse(GetRowValue(endAddress, cellAddress));
            var endCol = int.Parse(GetColValue(endAddress, cellAddress));

            for (var r = startRow; r <= endRow; r++)
            {
                for (var c = startCol; c <= endCol; c++)
                {
                    list.Add($"R{r}C{c}");
                }
            }

            return list;
        }

        public static bool IsAddressInRange(string address,
            string addressRange,
            SheetCellAddress cellAddress)
        {
            var arr = addressRange.Split(':').ToArray();
            if (arr.Length != 2)
                return false;

            var startAddress = arr[0];
            var endAddress = arr[1];

            var startRow = int.Parse(GetRowValue(startAddress, cellAddress));
            var startCol = int.Parse(GetColValue(startAddress, cellAddress));

            var endRow = int.Parse(GetRowValue(endAddress, cellAddress));
            var endCol = int.Parse(GetColValue(endAddress, cellAddress));

            var currentAddressRow = int.Parse(GetRowValue(address, cellAddress));
            var currentAddressCol = int.Parse(GetColValue(address, cellAddress));

            if (startRow <= currentAddressRow && endRow >= currentAddressRow &&
                startCol <= currentAddressCol && endCol >= currentAddressCol)
                return true;

            return false;
        }

        public static string NormalizeAddress(string address, ValueAddress formulaCellAddress)
        {
            var rowVal = GetRowValue(address, formulaCellAddress);
            var colVal = GetColValue(address, formulaCellAddress);

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

        public static string NormalizeAddress(string address, SheetCellAddress formulaCellAddress)
        {
            var rowVal = GetRowValue(address, formulaCellAddress);
            var colVal = GetColValue(address, formulaCellAddress);

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

        private static string GetRowValue(string address, ValueAddress formulaCellAddress)
        {
            if (string.IsNullOrEmpty(address))
                return null;

            var addressModel = new AddressModel(address, formulaCellAddress);
            if (addressModel.IsRowRelative)
                return (formulaCellAddress.Row + addressModel.RowValue).ToString();
            else
                return addressModel.RowValue.ToString();
        }

        private static string GetRowValue(string address, SheetCellAddress formulaCellAddress)
        {
            if (string.IsNullOrEmpty(address))
                return null;

            var addressModel = new AddressModel(address, formulaCellAddress);
            if (addressModel.IsRowRelative)
                return (formulaCellAddress.Row + addressModel.RowValue).ToString();
            else
                return addressModel.RowValue.ToString();
        }

        private static string GetColValue(string address, ValueAddress formulaCellAddress)
        {
            if (string.IsNullOrEmpty(address))
                return null;

            var addressModel = new AddressModel(address, formulaCellAddress);
            if (addressModel.IsColumnRelative)
                return (formulaCellAddress.Column + addressModel.ColumnValue).ToString();
            else
                return addressModel.ColumnValue.ToString();
        }

        private static string GetColValue(string address, SheetCellAddress formulaCellAddress)
        {
            if (string.IsNullOrEmpty(address))
                return null;

            var addressModel = new AddressModel(address, formulaCellAddress);
            if (addressModel.IsColumnRelative)
                return (formulaCellAddress.Column + addressModel.ColumnValue).ToString();
            else
                return addressModel.ColumnValue.ToString();
        }

        public static string GetCellAddress(SheetCellAddress cellAddress)
            => $"R{cellAddress.Row}C{cellAddress.Column}";

        #endregion
    }
}