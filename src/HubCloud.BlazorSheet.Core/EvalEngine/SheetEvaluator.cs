using System.Collections.Generic;
using DynamicExpresso;
using HubCloud.BlazorSheet.Core.EvalEngine.Abstract;
using Microsoft.Extensions.Logging;

namespace HubCloud.BlazorSheet.Core.EvalEngine
{
    public class SheetEvaluator
    {
        private readonly IEvaluatorLogger _logger;
        private Interpreter _interpreter;
        
        public IEnumerable<IEvaluatorLogMessage> Messages => _logger.Messages;
        public LogLevel LogLevel => _logger.MinimumLevel;

        public SheetEvaluator()
        {
            _logger = new EvaluatorLogger();
            _interpreter = InterpreterInitializer.CreateInterpreter();
        }
        
        public void SetLogLevel(LogLevel level)
        {
            _logger.MinimumLevel = level;
        }

        public void ClearLog()
        {
            _logger.Clear();
        }
    }
}