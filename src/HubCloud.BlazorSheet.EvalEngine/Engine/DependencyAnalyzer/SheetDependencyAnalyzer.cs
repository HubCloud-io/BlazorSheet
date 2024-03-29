﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Models;
using HubCloud.BlazorSheet.EvalEngine.Helpers;

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

        #region private methods
        
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