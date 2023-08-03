using System;

namespace HubCloud.BlazorSheet.Core.Events
{
    public class ColumnRemovedEventArgs
    {
        public Guid ColumnUid { get; set; }
        public int ColumnNumber { get; set; }
    }
}