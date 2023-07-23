using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetJoinedCellRange
    {
        private List<SheetCellAddress> _range;
        public SheetCell Cell { get; set; }
        public SheetCellAddress Address { get; set; }
        public List<SheetCellAddress> Range
        {
            get
            {
                if (_range == null)
                    _range = GetRange();

                return _range;
            }
        }

        private List<SheetCellAddress> GetRange()
        {
            var range = new List<SheetCellAddress>();

            var row = Address.Row;

            for (int i = 0; i < Cell.Rowspan; i++)
            {
                var column = Address.Column;

                for (int j = 0; j < Cell.Colspan; j++)
                {
                    range.Add(new SheetCellAddress(row, column));
                    column++;
                }

                row++;
            }

            return range;
        }
    }
}
