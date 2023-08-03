using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            var log = new List<ShiftLogModel>();
            var formulaCells = GetFormulaCells();
            foreach (var formulaCell in formulaCells)
            {
                var logItem = ProcessFormulaCell(formulaCell,
                    insertedRowIndex,
                    ShiftActions.Add,
                    ProcessRowAddresses,
                    ProcessRowRanges);
                
                log.Add(logItem);
            }

            return log;
        }

        public List<ShiftLogModel> OnColumnAdd(int insertedColumnIndex)
        {
            var log = new List<ShiftLogModel>();
            var formulaCells = GetFormulaCells();
            foreach (var formulaCell in formulaCells)
            {
                var logItem = ProcessFormulaCell(formulaCell,
                    insertedColumnIndex,
                    ShiftActions.Add,
                    ProcessColumnAddresses,
                    ProcessColumnRanges);
                
                log.Add(logItem);
            }
            
            return log;
        }

        public List<ShiftLogModel> OnRowDelete(int deletedRowIndex)
        {
            var log = new List<ShiftLogModel>();
            var formulaCells = GetFormulaCells();
            foreach (var formulaCell in formulaCells)
            {
                var logItem = ProcessFormulaCell(formulaCell,
                    deletedRowIndex,
                    ShiftActions.Delete,
                    ProcessRowAddresses,
                    ProcessRowRanges);
                
                log.Add(logItem);
            }

            return log;
        }

        public List<ShiftLogModel> OnColumnDelete(int deletedColumnIndex)
        {
            var log = new List<ShiftLogModel>();
            var formulaCells = GetFormulaCells();
            foreach (var formulaCell in formulaCells)
            {
                var logItem = ProcessFormulaCell(formulaCell,
                    deletedColumnIndex,
                    ShiftActions.Delete,
                    ProcessColumnAddresses,
                    ProcessColumnRanges);
                
                log.Add(logItem);
            }
            
            return log;
        }


        #region private methods

        private ShiftLogModel ProcessFormulaCell(SheetCell formulaCell,
            int insertedIndex,
            ShiftActions action,
            Action<StringBuilder, ShiftActions, Dictionary<string, string>,SheetCellAddress,int> processAddresses,
            Action<StringBuilder, ShiftActions, Dictionary<string, string>, SheetCellAddress, int> processRanges)
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

            processAddresses(formula, action, addressDict, formulaCellAddress, insertedIndex);
            processRanges(formula, action, rangeDict, formulaCellAddress, insertedIndex);

            var processedFormula = formula.ToString();
            formulaCell.Formula = processedFormula;

            log.CurrentFormula = processedFormula;

            return log;
        }

        private void ProcessRowAddresses(StringBuilder formula,
            ShiftActions action,
            Dictionary<string, string> addressDict,
            SheetCellAddress formulaAddress,
            int insertedRowIndex)
        {
            foreach (var address in addressDict)
            {
                var shiftedAddress = GetShiftedRowAddress(formulaAddress,
                    insertedRowIndex,
                    address.Value,
                    action);
                
                formula.Replace(address.Key, shiftedAddress.ToString());
            }
        }

        private void ProcessRowRanges(StringBuilder formula,
            ShiftActions action,
            Dictionary<string, string> rangeDict,
            SheetCellAddress formulaAddress,
            int insertedRowIndex)
        {
            foreach (var range in rangeDict)
            {
                var addresses = range.Value.Trim().Split(':');

                var shiftedAddress1 = GetShiftedRowAddress(formulaAddress,
                    insertedRowIndex,
                    addresses[0],
                    action);
                
                var shiftedAddress2 = GetShiftedRowAddress(formulaAddress,
                    insertedRowIndex,
                    addresses[1],
                    action);

                formula.Replace(range.Key, $"{shiftedAddress1}:{shiftedAddress2}");
            }
        }
        
        private void ProcessColumnAddresses(StringBuilder formula,
            ShiftActions action,
            Dictionary<string, string> addressDict,
            SheetCellAddress formulaAddress,
            int insertedColumnIndex)
        {
            foreach (var address in addressDict)
            {
                var shiftedAddress = GetShiftedColumnAddress(formulaAddress,
                    insertedColumnIndex,
                    address.Value,
                    action);
                formula.Replace(address.Key, shiftedAddress.ToString());
            }
        }
        
        private void ProcessColumnRanges(StringBuilder formula,
            ShiftActions action,
            Dictionary<string, string> rangeDict,
            SheetCellAddress formulaAddress,
            int insertedColumnIndex)
        {
            foreach (var range in rangeDict)
            {
                var addresses = range.Value.Trim().Split(':');

                var shiftedAddress1 = GetShiftedColumnAddress(formulaAddress,
                    insertedColumnIndex,
                    addresses[0],
                    action);
                
                var shiftedAddress2 = GetShiftedColumnAddress(formulaAddress,
                    insertedColumnIndex,
                    addresses[1],
                    action);

                formula.Replace(range.Key, $"{shiftedAddress1}:{shiftedAddress2}");
            }
        }
        
        private StringBuilder GetShiftedRowAddress(SheetCellAddress formulaAddress,
            int insertedRowIndex,
            string address,
            ShiftActions action)
        {
            var factor = 1;
            if (action == ShiftActions.Delete)
                factor = -1;
            
            string rValue;
            var addressModel = new AddressModel(address, formulaAddress);

            if (!addressModel.IsRowRelative)
            {
                if (addressModel.RowValue >= insertedRowIndex)
                    rValue = (addressModel.RowValue + factor).ToString();
                else
                    rValue = addressModel.RowValue.ToString();
            }
            else
            {
                var realRow = formulaAddress.Row + addressModel.RowValue;
                if (realRow < insertedRowIndex && formulaAddress.Row >= insertedRowIndex)
                    rValue = (addressModel.RowValue - factor).ToString();
                else if (realRow >= insertedRowIndex && formulaAddress.Row < insertedRowIndex)
                    rValue = (addressModel.RowValue + factor).ToString();
                else
                    rValue = addressModel.RowValue.ToString();
            }

            // build address
            var shiftedAddress = new StringBuilder();
            shiftedAddress.Append("R");
            if (addressModel.IsRowRelative)
                shiftedAddress.Append($"[{rValue}]");
            else
                shiftedAddress.Append($"{rValue}");

            shiftedAddress.Append("C");
            if (addressModel.IsColumnRelative)
                shiftedAddress.Append($"[{addressModel.ColumnValue.ToString()}]");
            else
                shiftedAddress.Append($"{addressModel.ColumnValue.ToString()}");
            return shiftedAddress;
        }
        
        private StringBuilder GetShiftedColumnAddress(SheetCellAddress formulaAddress,
            int insertedColumnIndex,
            string address,
            ShiftActions action)
        {
            var factor = 1;
            if (action == ShiftActions.Delete)
                factor = -1;
            
            string cValue;
            var addressModel = new AddressModel(address, formulaAddress);

            if (!addressModel.IsColumnRelative)
            {
                if (addressModel.ColumnValue >= insertedColumnIndex)
                    cValue = (addressModel.ColumnValue + factor).ToString();
                else
                    cValue = addressModel.ColumnValue.ToString();
            }
            else
            {
                var realRow = formulaAddress.Column + addressModel.ColumnValue;
                if (realRow < insertedColumnIndex && formulaAddress.Column >= insertedColumnIndex)
                    cValue = (addressModel.ColumnValue - factor).ToString();
                else if (realRow >= insertedColumnIndex && formulaAddress.Column < insertedColumnIndex)
                    cValue = (addressModel.ColumnValue + factor).ToString();
                else
                    cValue = addressModel.ColumnValue.ToString();
            }

            // build address
            var shiftedAddress = new StringBuilder();
            shiftedAddress.Append("R");
            if (addressModel.IsRowRelative)
                shiftedAddress.Append($"[{addressModel.RowValue.ToString()}]");
            else
                shiftedAddress.Append($"{addressModel.RowValue.ToString()}");

            shiftedAddress.Append("C");
            if (addressModel.IsColumnRelative)
                shiftedAddress.Append($"[{cValue}]");
            else
                shiftedAddress.Append($"{cValue}");
            return shiftedAddress;
        }
        
        private IEnumerable<SheetCell> GetFormulaCells()
            => _sheet.Cells.Where(x => !string.IsNullOrEmpty(x.Formula));

        #endregion
    }
}