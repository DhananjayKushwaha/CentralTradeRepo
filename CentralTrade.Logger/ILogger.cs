using System;
using System.Collections.Generic;

namespace CentralTrade.Logger
{
    public interface ILogger
    {
        void Log(LogSeverity logSeverity, string message);
    }
}
