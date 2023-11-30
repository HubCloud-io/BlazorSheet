using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetCellCopiedSettings
    {
        public string Format { get; set; }
        public Guid? EditSettingsUid { get; set; }
        public Guid? StyleUid { get; set; }
    }
}
