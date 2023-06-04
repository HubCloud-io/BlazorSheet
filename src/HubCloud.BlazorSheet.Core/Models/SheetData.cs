using System;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetData
    {
        private readonly int _rowsCount;
        private readonly int _columnsCount;
        private readonly object[,] _data;
        
        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }

        public SheetData(int rowsCount, int columnsCount)
        {
            _rowsCount = rowsCount;
            _columnsCount = columnsCount;

            _data = new object[_rowsCount, _columnsCount];
        }

        public SheetData(Sheet sheet)
        {
            _rowsCount = sheet.RowsCount;
            _columnsCount = sheet.ColumnsCount;

            _data = new object[_rowsCount, _columnsCount];

            foreach (var cell in sheet.Cells)
            {
                var cellAddress = sheet.CellAddress(cell);
                this[cellAddress.Row, cellAddress.Column] =  cell.Value;
            }
        }

        public object this[int row, int column]
        {
            get
            {
                if (row <= 0 || row > _rowsCount)
                    throw new IndexOutOfRangeException($"Row number {row} is out of range.");
                if (column <= 0 || column > _columnsCount)
                    throw new IndexOutOfRangeException($"Column number {column} is out of range.");

                return _data[row - 1, column - 1];
            }
            set
            {
                if (row <= 0 || row > _rowsCount)
                    throw new IndexOutOfRangeException($"Row number {row} is out of range.");
                if (column <= 0 || column > _columnsCount)
                    throw new IndexOutOfRangeException($"Column number {column} is out of range.");

                _data[row - 1, column - 1] = value;
            }
        }

        public UniversalValue GetValue(int row, int column)
        {
            return new UniversalValue(_data[row - 1, column - 1]);
        }
        
        public UniversalValue GetValue(string address)
        {
            var cellAddress = new SheetCellAddress(address, CurrentRow, CurrentColumn);
            return new UniversalValue(_data[cellAddress.Row - 1, cellAddress.Column - 1]);
        }

        public UniversalValue Sum(string address)
        {

            var total = new UniversalValue(0M);
            
            var range = new SheetRange(address, CurrentRow, CurrentColumn);

            for (var r = range.RowStart; r <= range.RowEnd; r++)
            {
                for (var c = range.ColumnStart; c <= range.ColumnEnd; c++)
                {
                    var uValue = GetValue(r, c);
                    total += uValue;
                }
            }

            return total;
        }
        
    }
}