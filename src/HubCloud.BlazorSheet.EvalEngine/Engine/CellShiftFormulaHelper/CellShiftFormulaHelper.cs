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
        private readonly List<SheetCell> _formulaCells;

        public CellShiftFormulaHelper(Sheet sheet)
        {
            _sheet = sheet;
            _formulaCells = _sheet.Cells
                .Where(x => !string.IsNullOrEmpty(x.Formula))
                .ToList();
        }

        public List<ShiftLogModel> OnRowAdd(int insertedRowNumber)
        {
            var log = new List<ShiftLogModel>();
            var formulaCells = GetFormulaCells(exceptRowNumber: insertedRowNumber);
            foreach (var formulaCell in formulaCells)
            {
                var logItem = ProcessFormulaCell(formulaCell,
                    insertedRowNumber,
                    ShiftActions.Add,
                    ProcessRowAddresses,
                    ProcessRowRanges);
                
                log.Add(logItem);
            }

            return log;
        }

        public List<ShiftLogModel> OnColumnAdd(int insertedColumnNumber)
        {
            var log = new List<ShiftLogModel>();
            var formulaCells = GetFormulaCells(exceptColumnNumber: insertedColumnNumber);
            foreach (var formulaCell in formulaCells)
            {
                var logItem = ProcessFormulaCell(formulaCell,
                    insertedColumnNumber,
                    ShiftActions.Add,
                    ProcessColumnAddresses,
                    ProcessColumnRanges);
                
                log.Add(logItem);
            }
            
            return log;
        }

        public List<ShiftLogModel> OnRowDelete(int deletedRowNumber)
        {
            var log = new List<ShiftLogModel>();
            var formulaCells = GetFormulaCells();
            foreach (var formulaCell in formulaCells)
            {
                var logItem = ProcessFormulaCell(formulaCell,
                    deletedRowNumber,
                    ShiftActions.Delete,
                    ProcessRowAddresses,
                    ProcessRowRanges);
                
                log.Add(logItem);
            }

            return log;
        }

        public List<ShiftLogModel> OnColumnDelete(int deletedColumnNumber)
        {
            var log = new List<ShiftLogModel>();
            var formulaCells = GetFormulaCells();
            foreach (var formulaCell in formulaCells)
            {
                var logItem = ProcessFormulaCell(formulaCell,
                    deletedColumnNumber,
                    ShiftActions.Delete,
                    ProcessColumnAddresses,
                    ProcessColumnRanges);
                
                log.Add(logItem);
            }
            
            return log;
        }


        #region private methods

        private ShiftLogModel ProcessFormulaCell(SheetCell formulaCell,
            int insertedNumber,
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

            processAddresses(formula, action, addressDict, formulaCellAddress, insertedNumber);
            processRanges(formula, action, rangeDict, formulaCellAddress, insertedNumber);

            var processedFormula = formula.ToString();
            formulaCell.Formula = processedFormula;

            log.CurrentFormula = processedFormula;

            return log;
        }

        private void ProcessRowAddresses(StringBuilder formula,
            ShiftActions action,
            Dictionary<string, string> addressDict,
            SheetCellAddress formulaAddress,
            int insertedRowNumber)
        {
            foreach (var address in addressDict)
            {
                var shiftedAddress = GetShiftedRowAddress(formulaAddress,
                    insertedRowNumber,
                    address.Value,
                    action);
                
                formula.Replace(address.Key, shiftedAddress.ToString());
            }
        }

        private void ProcessRowRanges(StringBuilder formula,
            ShiftActions action,
            Dictionary<string, string> rangeDict,
            SheetCellAddress formulaAddress,
            int insertedRowNumber)
        {
            foreach (var range in rangeDict)
            {
                var addresses = range.Value.Trim().Split(':');

                var shiftedAddress1 = GetShiftedRowAddress(formulaAddress,
                    insertedRowNumber,
                    addresses[0],
                    action);
                
                var shiftedAddress2 = GetShiftedRowAddress(formulaAddress,
                    insertedRowNumber,
                    addresses[1],
                    action);

                formula.Replace(range.Key, $"{shiftedAddress1}:{shiftedAddress2}");
            }
        }
        
        private void ProcessColumnAddresses(StringBuilder formula,
            ShiftActions action,
            Dictionary<string, string> addressDict,
            SheetCellAddress formulaAddress,
            int insertedColumnNumber)
        {
            foreach (var address in addressDict)
            {
                var shiftedAddress = GetShiftedColumnAddress(formulaAddress,
                    insertedColumnNumber,
                    address.Value,
                    action);
                formula.Replace(address.Key, shiftedAddress.ToString());
            }
        }
        
        private void ProcessColumnRanges(StringBuilder formula,
            ShiftActions action,
            Dictionary<string, string> rangeDict,
            SheetCellAddress formulaAddress,
            int insertedColumnNumber)
        {
            foreach (var range in rangeDict)
            {
                var addresses = range.Value.Trim().Split(':');

                var shiftedAddress1 = GetShiftedColumnAddress(formulaAddress,
                    insertedColumnNumber,
                    addresses[0],
                    action);
                
                var shiftedAddress2 = GetShiftedColumnAddress(formulaAddress,
                    insertedColumnNumber,
                    addresses[1],
                    action);

                formula.Replace(range.Key, $"{shiftedAddress1}:{shiftedAddress2}");
            }
        }
        
        private StringBuilder GetShiftedRowAddress(SheetCellAddress formulaAddress,
            int insertedRowNumber,
            string address,
            ShiftActions action)
        {
            var factor = 1;
            if (action == ShiftActions.Delete)
                factor = -1;
            
            string rValue;
            var addressModel = new AddressModel(address, formulaAddress);

            if (addressModel.IsRowRelative)
            {
                var realRow = formulaAddress.Row + addressModel.RowValue;
                if (realRow < insertedRowNumber && formulaAddress.Row >= insertedRowNumber)
                    rValue = (addressModel.RowValue - factor).ToString();
                else if (realRow >= insertedRowNumber && formulaAddress.Row < insertedRowNumber)
                    rValue = (addressModel.RowValue + factor).ToString();
                else
                    rValue = addressModel.RowValue.ToString();
            }
            else
            {
                if (addressModel.RowValue >= insertedRowNumber)
                    rValue = (addressModel.RowValue + factor).ToString();
                else
                    rValue = addressModel.RowValue.ToString();
            }

            // build address
            var shiftedAddress = new StringBuilder();
            shiftedAddress.Append("R");
            if (addressModel.IsRowRelative)
                shiftedAddress.Append($"[{rValue}]");
            else if (!addressModel.IsRowCurrent)
                shiftedAddress.Append($"{rValue}");

            shiftedAddress.Append("C");
            if (addressModel.IsColumnRelative)
                shiftedAddress.Append($"[{addressModel.ColumnValue.ToString()}]");
            else if (!addressModel.IsColumnCurrent)
                shiftedAddress.Append($"{addressModel.ColumnValue.ToString()}");
            
            return shiftedAddress;
        }
        
        private StringBuilder GetShiftedColumnAddress(SheetCellAddress formulaAddress,
            int insertedColumnNumber,
            string address,
            ShiftActions action)
        {
            var factor = 1;
            if (action == ShiftActions.Delete)
                factor = -1;
            
            string cValue;
            var addressModel = new AddressModel(address, formulaAddress);

            if (addressModel.IsColumnRelative)
            {
                var realRow = formulaAddress.Column + addressModel.ColumnValue;
                if (realRow < insertedColumnNumber && formulaAddress.Column >= insertedColumnNumber)
                    cValue = (addressModel.ColumnValue - factor).ToString();
                else if (realRow >= insertedColumnNumber && formulaAddress.Column < insertedColumnNumber)
                    cValue = (addressModel.ColumnValue + factor).ToString();
                else
                    cValue = addressModel.ColumnValue.ToString();
            }
            else
            {
                if (addressModel.ColumnValue >= insertedColumnNumber)
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

        private IEnumerable<SheetCell> GetFormulaCells(int? exceptRowNumber = null,
            int? exceptColumnNumber = null)
        {
            // var query = _sheet.Cells.Where(x => !string.IsNullOrEmpty(x.Formula));
            var query = _formulaCells.AsEnumerable();

            if (exceptRowNumber != null)
                query = query.Where(x => _sheet.CellAddress(x).Row != exceptRowNumber);
            
            if (exceptColumnNumber != null)
                query = query.Where(x => _sheet.CellAddress(x).Column != exceptColumnNumber);
            
            return query;
        }

        #endregion
    }
}