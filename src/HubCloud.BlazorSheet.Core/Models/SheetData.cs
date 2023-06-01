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
        
        public object this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= _rowsCount)
                    throw new IndexOutOfRangeException($"Row index {row} is out of range.");
                if (column < 0 || column >= _columnsCount)
                    throw new IndexOutOfRangeException($"Column index {column} is out of range.");

                return _data[row, column];
            }
            set
            {
                if (row < 0 || row >= _rowsCount)
                    throw new IndexOutOfRangeException($"Row index {row} is out of range.");
                if (column < 0 || column >= _columnsCount)
                    throw new IndexOutOfRangeException($"Column index {column} is out of range.");

                _data[row, column] = value;
            }
        }
    }
}