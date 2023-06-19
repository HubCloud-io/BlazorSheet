using System;
using Microsoft.Extensions.Logging;

namespace HubCloud.BlazorSheet.EvalEngine.Abstract
{
    public interface IEvaluatorLogMessage
    {
        DateTime Period { get; set; }
        LogLevel Level { get; set; }
        string Text { get; set; }
    }
}