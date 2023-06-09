using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class WorkbookSettings
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
        public List<SheetSettings> Sheets { get; set; } = new List<SheetSettings>();
    }
}
