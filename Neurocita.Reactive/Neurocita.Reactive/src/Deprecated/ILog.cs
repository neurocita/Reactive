using System;

namespace Neurocita.Reactive
{
    public interface ILog
    {
        string Source { get; set; }

        void Trace(string message, params object[] arguments);
        void Info(string message, params object[] arguments);
        void Warn(string message, params object[] arguments);
        void Warn(Exception exception);
        void Warn(Exception exception, string message, params object[] arguments);
        void Error(string message, params object[] arguments);
        void Error(Exception exception);
        void Error(Exception exception, string message, params object[] arguments);
        void Entry(LogEntry logEntry);
    }
}
