using System;
using System.Collections.Generic;
using HubCloud.BlazorSheet.EvalEngine.Abstract;
using Microsoft.Extensions.Logging;

namespace HubCloud.BlazorSheet.EvalEngine.Engine
{
     public class EvaluatorLogger : IEvaluatorLogger
    {
        private List<IEvaluatorLogMessage> _messages = new List<IEvaluatorLogMessage>();

        public LogLevel MinimumLevel { get; set; } = LogLevel.Warning;
        public IEnumerable<IEvaluatorLogMessage> Messages => _messages;

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= MinimumLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);
            _messages.Add(new EvaluatorLogMessage() { Level = logLevel, Text = message });
        }

        public void Log(LogLevel level, string messageTemplate, params object[] args)
        {
            if (!IsEnabled(level))
            {
                return;
            }

            var messageText = string.Format(messageTemplate, args);

            _messages.Add(new EvaluatorLogMessage() { Level = level, Text = messageText });
        }

        public void LogTrace(string messageTemplate, params object[] args)
        {
            Log(LogLevel.Trace, messageTemplate, args);
        }

        public void LogDebug(string messageTemplate, params object[] args)
        {
            Log(LogLevel.Debug, messageTemplate, args);
        }
        
        public void LogInformation(string messageTemplate, params object[] args)
        {
            Log(LogLevel.Information, messageTemplate, args);
        }
        
        public void LogWarning(string messageTemplate, params object[] args)
        {
            Log(LogLevel.Warning, messageTemplate, args);
        }
        
        public void LogError(string messageTemplate, params object[] args)
        {
            Log(LogLevel.Error, messageTemplate, args);
        }
        
        public void LogCritical(string messageTemplate, params object[] args)
        {
            Log(LogLevel.Critical, messageTemplate, args);
        }

        public void Clear()
        {
            _messages.Clear();
        }
    }
}