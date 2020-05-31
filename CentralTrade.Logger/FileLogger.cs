using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace CentralTrade.Logger
{
    /// <summary>
    /// Thread safe simple file logger implementation
    /// </summary>
    public class FileLogger : ILogger
    {
        private TextWriter _textWriter;
        private static Mutex _mutex = new Mutex();
        private string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(LogSeverity logSeverity, string message)
        {
            try
            {
                //used mutex here to handle OS level lock. In case UI app is reading data from same file and showing in UI
                _mutex.WaitOne();

                using (_textWriter = TextWriter.Synchronized(File.AppendText(_filePath)))
                {
                    _textWriter.WriteLine(string.Format("{0}, {1}", logSeverity, message));
                    _textWriter.Close();
                    _textWriter.Flush();
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        ~FileLogger()
        {
            _mutex.Dispose();
        }
    }
}
