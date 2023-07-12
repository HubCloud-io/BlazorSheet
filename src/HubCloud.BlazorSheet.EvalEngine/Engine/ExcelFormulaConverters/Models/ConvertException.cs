using System;
using System.Collections.Generic;
using System.Text;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.ExcelFormulaConverter.Models
{
    public class ConvertException
    {
        public string Statement { get; set; }
        public string ExceptionDescription { get; set; }
    }
}
