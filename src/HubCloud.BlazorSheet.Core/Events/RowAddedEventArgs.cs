using System;
using System.Security.Cryptography.X509Certificates;

namespace HubCloud.BlazorSheet.Core.Events
{
    public class RowAddedEventArgs
    {
        public Guid SourceUid { get; set; }
        public Guid RowUid { get; set; }
    }
}