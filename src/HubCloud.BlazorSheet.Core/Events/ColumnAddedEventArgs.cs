using System;

namespace HubCloud.BlazorSheet.Core.Events
{
    public class ColumnAddedEventArgs
    {
        public Guid SourceUid { get; set; }
        public Guid ColumnUid { get; set; }
        public int Index { get; set; }
    }
}