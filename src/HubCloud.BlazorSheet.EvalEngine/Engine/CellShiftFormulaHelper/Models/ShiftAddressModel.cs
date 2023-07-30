using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.CellShiftFormulaHelper.Models
{
    public class ShiftAddressModel
    {
        private readonly SheetCellAddress _sheetCellAddress;
        private readonly SheetRange _sheetRange;
        private readonly int _formulaRow;
        private readonly int _formulaColumn;

        public ShiftAddressModel(string placeholder,
            string address,
            int formulaRow,
            int formulaColumn,
            bool isRange = false)
        {
            _formulaRow = formulaRow;
            _formulaColumn = formulaColumn;

            Placeholder = placeholder;
            Address = address;
            IsRange = isRange;

            if (!IsRange)
                _sheetCellAddress = new SheetCellAddress(Address, _formulaRow, _formulaColumn);
            else
                _sheetRange = new SheetRange(Address, _formulaRow, _formulaColumn);
        }

        public string Placeholder { get; set; }
        public string Address { get; set; }
        public bool IsRange { get; set; }

        public string GetShifted(int insertedRowIndex)
            => IsRange ?
                GetShiftedRange(insertedRowIndex) :
                GetShiftedAddress(insertedRowIndex, Address, _sheetCellAddress);

        private string GetShiftedAddress(int insertedRowIndex, string address, SheetCellAddress sheetCellAddress)
        {
            if (sheetCellAddress.Row < insertedRowIndex)
                return address;

            if (!sheetCellAddress.RowIsRelative)
                return $"R{sheetCellAddress.Row + 1}C{sheetCellAddress.Column}";

            return string.Empty; // ToDo: logic for relative address
        }

        private string GetShiftedRange(int insertedRowIndex)
        {
            var startAddress = GetShiftedAddress(insertedRowIndex, _sheetRange.StartAddress.ToString(), _sheetRange.StartAddress);
            var endAddress = GetShiftedAddress(insertedRowIndex, _sheetRange.EndAddress.ToString(), _sheetRange.EndAddress);

            return $"{startAddress}:{endAddress}";
        }
    }
}