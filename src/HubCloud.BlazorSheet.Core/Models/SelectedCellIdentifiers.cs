using System;
using System.Collections.Generic;
using System.Linq;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SelectedCellIdentifiers
    {
        private HashSet<Guid> _hash = new HashSet<Guid>();
        
        public void Clear(Sheet sheet)
        {
            foreach (var uid in _hash)
            {
                var cell = sheet.Cells.FirstOrDefault(x => x.Uid == uid);
                if (cell != null)
                {
                    cell.IsSelected = false;
                }
            }
            _hash.Clear();
        }

        public void Add(SheetCell cell)
        {
            cell.IsSelected = true;
            _hash.Add(cell.Uid);
        }

        public void Remove(SheetCell cell)
        {
            cell.IsSelected = false;
            _hash.Remove(cell.Uid);
        }

        public bool Contains(SheetCell cell)
        {
            return _hash.Contains(cell.Uid);
        }
    }
}