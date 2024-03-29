﻿using System.Collections.Generic;

namespace HubCloud.BlazorSheet.EvalEngine.Engine.FormulaProcessors.Models
{
    public class Statement
    {
        public ElementType Type { get; set; }
        public string OriginStatement { get; set; }
        public string ProcessedStatement { get; set; }
        public string FunctionName { get; set; }
        public List<FunctionParam> FunctionParams { get; set; }
    }
}