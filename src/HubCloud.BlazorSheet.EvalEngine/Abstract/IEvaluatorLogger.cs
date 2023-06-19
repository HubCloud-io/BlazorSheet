using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace HubCloud.BlazorSheet.EvalEngine.Abstract
{
    public interface IEvaluatorLogger : ILogger
    {
        LogLevel MinimumLevel { get; set; }
        IEnumerable<IEvaluatorLogMessage> Messages { get; }
        void Log(LogLevel level, string messageTemplate, params object[] args);
        void LogTrace(string messageTemplate, params object[] args);
        void LogDebug(string messageTemplate, params object[] args);
        void LogInformation(string messageTemplate, params object[] args);
        void LogWarning(string messageTemplate, params object[] args);
        void LogError(string messageTemplate, params object[] args);
        void LogCritical(string messageTemplate, params object[] args);
        void Clear();

    }
}