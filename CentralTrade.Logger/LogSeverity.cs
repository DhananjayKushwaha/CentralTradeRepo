using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralTrade.Logger
{
    public enum LogSeverity
    {
        Info,
        Debug,
        Warning,
        Error
    }

    public static class LogSeverityExtensions
    {
        public static List<LogSeverity> All()
        {
            return Enum.GetValues(typeof(LogSeverity)).Cast<LogSeverity>().ToList();
        }
    }
}
