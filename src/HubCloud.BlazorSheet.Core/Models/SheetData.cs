using System;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetData
    {
        private readonly int _rowsCount;
        private readonly int _columnsCount;
        private readonly object[,] _data;

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
                var cellCoordinates = sheet.CellCoordinates(cell);
                this[cellCoordinates.Item1, cellCoordinates.Item2] = new UniversalValue(cell.Value);
            }
        }

        public UniversalValue this[int row, int column]
        {
            get
            {
                if (row <= 0 || row > _rowsCount)
                    throw new IndexOutOfRangeException($"Row number {row} is out of range.");
                if (column <= 0 || column > _columnsCount)
                    throw new IndexOutOfRangeException($"Column number {column} is out of range.");

                return new UniversalValue(_data[row - 1, column - 1]);
            }
            set
            {
                if (row <= 0 || row > _rowsCount)
                    throw new IndexOutOfRangeException($"Row number {row} is out of range.");
                if (column <= 0 || column > _columnsCount)
                    throw new IndexOutOfRangeException($"Column number {column} is out of range.");

                if (value is UniversalValue uValue)
                {
                    _data[row - 1, column - 1] = uValue.Value;
                }
                else
                {
                    _data[row - 1, column - 1] = value;
                }
            }
        }
    }
}