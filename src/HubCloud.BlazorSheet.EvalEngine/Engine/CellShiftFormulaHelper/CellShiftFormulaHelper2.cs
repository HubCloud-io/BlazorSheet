﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HubCloud.BlazorSheet.Core.Models;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Abstractions;
using HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Models;
using HubCloud.BlazorSheet.EvalEngine.Helpers;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper
{
    public class CellShiftFormulaHelper2 : IFormulaShifter
    {
        private readonly Sheet _sheet;

        public CellShiftFormulaHelper2(Sheet sheet)
        {
            _sheet = sheet;
        }

        public List<ShiftLogModel> OnRowAdd(int insertedRowIndex)
        {
            var log = new List<ShiftLogModel>();

            var formulaCells = _sheet.Cells
                .Where(x => !string.IsNullOrEmpty(x.Formula))
                .ToList();

            foreach (var formulaCell in formulaCells)
            {
                var logItem = ProcessFormulaCell(formulaCell, insertedRowIndex);
                log.Add(logItem);
            }

            return log;
        }

        public List<ShiftLogModel> OnColumnAdd(int insertedColumnIndex)
        {
            throw new System.NotImplementedException();
        }


        #region private methods

        private ShiftLogModel ProcessFormulaCell(SheetCell formulaCell, int insertedRowIndex)
        {
            var formulaCellAddress = _sheet.CellAddress(formulaCell);
            
            var log = new ShiftLogModel
            {
                PrevFormula = formulaCell.Formula,
                CellAddress = formulaCellAddress.ToString()
            };

            var formula = new StringBuilder(formulaCell.Formula);

            var rangeDict = new Dictionary<string, string>();
            var addressDict = new Dictionary<string, string>();

            // check address ranges
            var rangeRegex = RegexHelper.AddressRangeRegex;
            var rangeMatchesList = rangeRegex.Matches(formula.ToString())
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct();

            var i = 0;
            foreach (var match in rangeMatchesList)
            {
                var placeholder = $@"{{RNG{i}}}";
                formula = formula.Replace(match, placeholder);
                rangeDict.Add(placeholder, match);
                i++;
            }

            // check simple addresses
            var addressRegex = RegexHelper.AddressRegex;
            var addressMatchesList = addressRegex.Matches(formula.ToString())
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct();

            i = 0;
            foreach (var match in addressMatchesList)
            {
                var placeholder = $@"{{ADR{i++}}}";
                formula = formula.Replace(match, placeholder);
                addressDict.Add(placeholder, match);
            }

            ProcessAddresses(formula, addressDict, formulaCellAddress, insertedRowIndex);
            ProcessRanges(formula, rangeDict, formulaCellAddress, insertedRowIndex);

            var processedFormula = formula.ToString();
            formulaCell.Formula = processedFormula;

            log.CurrentFormula = processedFormula;

            return log;
        }

        private void ProcessAddresses(StringBuilder formula,
            Dictionary<string, string> addressDict,
            SheetCellAddress formulaAddress,
            int insertedRowIndex)
        {
            foreach (var addr in addressDict)
            {
                string rValue;
                string cValue;

                // row
                var addressModel = new AddressModel(addr.Value);

                if (!addressModel.IsRowRelative)
                {
                    if (addressModel.RowValue >= insertedRowIndex)
                        rValue = (addressModel.RowValue + 1).ToString();
                    else
                        rValue = addressModel.RowValue.ToString();
                }
                else
                {
                    var realRow = formulaAddress.Row + addressModel.RowValue;
                    if (realRow < insertedRowIndex && formulaAddress.Row >= insertedRowIndex)
                        rValue = (addressModel.RowValue - 1).ToString();
                    else if (realRow >= insertedRowIndex && formulaAddress.Row < insertedRowIndex)
                        rValue = (addressModel.RowValue + 1).ToString();
                    else
                        rValue = addressModel.RowValue.ToString();
                }

                // columns
                cValue = addressModel.ColumnValue.ToString();

                // build address
                var shiftedAddress = new StringBuilder();
                shiftedAddress.Append("R");
                if (addressModel.IsRowRelative)
                    shiftedAddress.Append($"[{rValue}]");
                else
                    shiftedAddress.Append($"{rValue}");

                shiftedAddress.Append("C");
                if (addressModel.IsColumnRelative)
                    shiftedAddress.Append($"[{cValue}]");
                else
                    shiftedAddress.Append($"{cValue}");

                formula.Replace(addr.Key, shiftedAddress.ToString());
            }
        }

        private StringBuilder ProcessRanges(StringBuilder formula,
            Dictionary<string, string> rangeDict,
            SheetCellAddress formulaAddress,
            int insertedRowIndex)
        {
            return formula;
        }

        #endregion
    }
}