using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Abstractions;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Models;
using HubCloud.BlazorSheet.EvalEngine.Helpers;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper
{
    public class CellShiftFormulaHelper : IFormulaShifter
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
            var formulaModel = ExtractAddresses(formulaCell);

            // build formula
            var formula = formulaModel.ReplacedFormula;
            foreach (var addressModel in formulaModel.AddressList)
            {
                formula.Replace(addressModel.Placeholder, addressModel.GetShifted(insertedRowIndex));
            }

            var shiftedFormula = formula.ToString();
            
            var logItem = new ShiftLogModel
            {
                PrevFormula = formulaCell.Formula,
                CurrentFormula = shiftedFormula,
                CellAddress = _sheet.CellAddress(formulaCell).ToString()
            };

            formulaCell.Formula = shiftedFormula;

            return logItem;
        }

        private FormulaModel ExtractAddresses(SheetCell formulaCell)
        {
            var cellAddress = _sheet.CellAddress(formulaCell);
            var formula = new StringBuilder(formulaCell.Formula);
            var formulaRow = cellAddress.Column;
            var formulaColumn = cellAddress.Row;

            var model = new FormulaModel();

            // check address ranges
            var rangeRegex = RegexHelper.AddressRangeRegex;
            var rangeMatchesList = rangeRegex.Matches(formula.ToString())
                .Cast<Match>()
                .Distinct()
                .Select((x, i) =>
                    new ShiftAddressModel($"{{RNG{i}}}", x.Value, formulaRow, formulaColumn, isRange: true))
                .ToList();

            foreach (var match in rangeMatchesList)
            {
                formula.Replace(match.Address, match.Placeholder);
            }

            // check simple addresses
            var addressRegex = RegexHelper.AddressRegex;
            var addressMatchesList = addressRegex.Matches(formula.ToString())
                .Cast<Match>()
                .Distinct()
                .Select((x, i) => new ShiftAddressModel($"{{ADR{i}}}", x.Value, formulaRow, formulaColumn))
                .ToList();

            foreach (var match in addressMatchesList)
            {
                formula.Replace(match.Address, match.Placeholder);
            }

            model.AddressList.AddRange(rangeMatchesList);
            model.AddressList.AddRange(addressMatchesList);
            model.ReplacedFormula = formula;

            return model;
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
                .Any(x => CheckRange(x, insertedRowIndex));

            if (isRangeMatches)
                return true;

            // check simple addresses
            var addressRegex = RegexHelper.AddressRegex;
            var isAddressMatches = addressRegex.Matches(formula)
                .Cast<Match>()
                .Select(x => new SheetCellAddress(x.Value, formulaRow, formulaColumn))
                .Any(x => CheckAddress(x, insertedRowIndex));

            if (isAddressMatches)
                return true;

            return false;
        }

        private bool CheckAddress(SheetCellAddress cell,
            int insertedRowIndex)
        {
            // if (!cell.RowIsRelative)
            //     return cell.Row >= insertedRowIndex;

            return true;
        }
        
        private bool CheckRange(SheetRange range, int insertedRowIndex)
        {
            return range.RowStart >= insertedRowIndex || range.RowEnd >= insertedRowIndex;
        }

        #endregion
    }
}