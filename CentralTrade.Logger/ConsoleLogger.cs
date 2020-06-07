using System;
using System.Collections.Generic;
using System.Text;

namespace CentralTrade.Logger
{
    public class ConsoleLogger : Logger
    {
        public ConsoleLogger(List<LogSeverity> logSeverities, ILogger nextLogger = null) : base(logSeverities, nextLogger)
        {

        }

        protected override void LogMessage(LogSeverity logSeverity, string message) 
        {
            Console.WriteLine(string.Format("[{0}] - {1}", logSeverity, message));
        }
    }
}
