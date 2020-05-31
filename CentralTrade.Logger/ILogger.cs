using System;

namespace CentralTrade.Logger
{
    public interface ILogger
    {
        void Log(LogSeverity logSeverity, string Message);
    }
}
