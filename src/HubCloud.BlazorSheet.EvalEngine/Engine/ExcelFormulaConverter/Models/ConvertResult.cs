using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverter.Models
{
    public class ConvertResult
    {
        public string Formula { get; set; }
        public IEnumerable<ConvertException> ConvertExceptions { get; set; }
    }
}
