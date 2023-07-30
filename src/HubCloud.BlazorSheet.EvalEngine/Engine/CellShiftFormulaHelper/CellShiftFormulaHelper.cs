using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Models;
using HubCloud.BlazorSheet.EvalEngine.Helpers;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper
{
    public class ShiftAddressModel
    {
        public string Placeholder { get; set; }
        public string Address { get; set; }
        public bool IsRange { get; set; }
        public SheetCellAddress SheetCellAddress { get; set; }
        public SheetRange SheetRange { get; set; }
    }
    
    public class CellShiftFormulaHelper
    {
        private readonly Sheet _sheet;

        public CellShiftFormulaHelper(Sheet sheet)
        {
            _sheet = sheet;
        }

        public List<ShiftLogModel> OnRowAdd(int insertedRowIndex)
        {
            var shiftLog = _sheet.Cells
                .Where(x => !string.IsNullOrEmpty(x.Formula) &&
                            IsFormulaContainsShiftedCells(x, insertedRowIndex))
                .Select(x => ShiftAddresses(x, insertedRowIndex))
                .ToList();

            return shiftLog;
        }

        public List<ShiftLogModel> OnColumnAdd(int insertedColumnIndex)
        {
            throw new NotImplementedException();
        }

        #region private methods
        
        private ShiftLogModel ShiftAddresses(SheetCell formulaCell, int insertedRowIndex)
        {
            var cellAddress = _sheet.CellAddress(formulaCell);
            var formula = formulaCell.Formula;
            var formulaRow = cellAddress.Column;
            var formulaColumn = cellAddress.Row;

            var addressList = new List<ShiftAddressModel>();
            
            // check address ranges
            var rangeRegex = RegexHelper.AddressRangeRegex;
            var rangeMatchesList = rangeRegex.Matches(formula)
                .Cast<Match>()
                .Distinct()
                .Select((x, i) => new ShiftAddressModel
                {
                    Placeholder = $"RNG{i}",
                    IsRange = true,
                    Address = x.Value,
                    SheetRange = new SheetRange(x.Value, formulaRow, formulaColumn)
                });

            // check simple addresses
            var addressRegex = RegexHelper.AddressRegex;
            var addressMatchesList = addressRegex.Matches(formula)
                .Cast<Match>()
                .Distinct()
                .Select((x, i) => new ShiftAddressModel
                {
                    Placeholder = $"ADR{i}",
                    IsRange = false,
                    Address = x.Value,
                    SheetCellAddress = new SheetCellAddress(x.Value, formulaRow, formulaColumn)
                });
            
            addressList.AddRange(rangeMatchesList);
            addressList.AddRange(addressMatchesList);
            
            //...

            return null;
        }
        
        protected bool IsFormulaContainsShiftedCells(SheetCell formulaCell, int insertedRowIndex)
        {
            var cellAddress = _sheet.CellAddress(formulaCell);
            var formula = formulaCell.Formula;
            var formulaRow = cellAddress.Column;
            var formulaColumn = cellAddress.Row;

            // check address ranges
            var rangeRegex = RegexHelper.AddressRangeRegex;
            var isRangeMatches = rangeRegex.Matches(formula)
                .Cast<Match>()
                .Select(x => new SheetRange(x.Value, formulaRow, formulaColumn))
                .Any(x => x.RowStart >= insertedRowIndex || x.RowEnd >= insertedRowIndex);
            
            if (isRangeMatches)
                return true;
            
            // check simple addresses
            var addressRegex = RegexHelper.AddressRegex;
            var isAddressMatches = addressRegex.Matches(formula)
                .Cast<Match>()
                .Select(x => new SheetCellAddress(x.Value, formulaRow, formulaColumn))
                .Any(x => x.Row >= insertedRowIndex);

            if (isAddressMatches)
                return true;

            return false;
        }

        #endregion
    }
}