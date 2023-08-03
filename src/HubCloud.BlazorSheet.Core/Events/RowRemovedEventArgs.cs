using System;

namespace HubCloud.BlazorSheet.Core.Events
{
    public class RowRemovedEventArgs
    {
        public Guid RowUid { get; set; }
        public int RowNumber { get; set; }
    }
}