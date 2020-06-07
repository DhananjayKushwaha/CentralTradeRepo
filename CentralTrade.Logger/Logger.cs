using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralTrade.Logger
{
    public abstract class Logger : ILogger
    {
        //logger chain instance
        protected ILogger _nextLogger;
        private List<LogSeverity> _logSeverities = Enum.GetValues(typeof(LogSeverity)).Cast<LogSeverity>().ToList();

        public Logger(List<LogSeverity> logSeverities, ILogger nextLogger)
        {
            _logSeverities = logSeverities;
            _nextLogger = nextLogger;
        }

        protected abstract void LogMessage(LogSeverity logSeverity, string message);

        public void Log(LogSeverity logSeverity, string message)
        {
            //if severity is supported then log else pass it to next logger in the chain
            if (_logSeverities.Contains(logSeverity))
            {
                LogMessage(logSeverity, message);
            }

            if(_nextLogger != null)
            {
                _nextLogger.Log(logSeverity, message);
            }
        }
    }
}
