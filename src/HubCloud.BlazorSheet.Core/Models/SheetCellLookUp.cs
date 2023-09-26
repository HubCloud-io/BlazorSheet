using System;
using System.Collections.Generic;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetCellLookUp
    {
        private Dictionary<Guid, Dictionary<Guid, SheetCell>> _lookup = new Dictionary<Guid, Dictionary<Guid, SheetCell>>();

        public void Add(SheetCell cell)
        {
            if (!_lookup.ContainsKey(cell.RowUid))
            {
                _lookup[cell.RowUid] = new Dictionary<Guid, SheetCell>();
            }

            _lookup[cell.RowUid][cell.ColumnUid] = cell;
        }
        
        public SheetCell Get(Guid rowUid, Guid columnUid)
        {
            if (_lookup.TryGetValue(rowUid, out var row))
            {
                if (row.TryGetValue(columnUid, out var cell))
                {
                    return cell;
                }
            }

            return null; // Cell not found
        }

        public void Remove(SheetCell cell)
        {
            if (_lookup.TryGetValue(cell.RowUid, out var row))
            {
                if (row.ContainsKey(cell.ColumnUid))
                {
                    row.Remove(cell.ColumnUid);
                }
            }
        }

        public void Clear()
        {
            _lookup.Clear();
        }
    }
}