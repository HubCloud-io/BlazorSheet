using System;
using HubCloud.BlazorSheet.Core.EvalEngine.Abstract;
using Microsoft.Extensions.Logging;

namespace HubCloud.BlazorSheet.Core.EvalEngine
{
    public class EvaluatorLogMessage : IEvaluatorLogMessage
    {
        public DateTime Period { get; set; } = DateTime.UtcNow;
        public LogLevel Level { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"{Period:yyyy-MM-dd hh:mm:ss.fff}.{Level}. {Text}.";
        }
    }
}