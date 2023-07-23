using System;

namespace HubCloud.BlazorSheet.Core.Models
{
    public struct CellKey
    {
        public Guid RowUid { get; set; }
        public Guid ColumnUid { get; set; }

        public CellKey(Guid rowUid, Guid columnUid)
        {
            RowUid = rowUid;
            ColumnUid = columnUid;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is CellKey other)
            {
                return RowUid == other.RowUid && ColumnUid == other.ColumnUid;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return RowUid.GetHashCode() * 397 ^ ColumnUid.GetHashCode();
            }
        }
    }
}