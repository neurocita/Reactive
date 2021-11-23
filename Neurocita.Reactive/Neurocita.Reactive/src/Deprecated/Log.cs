using System;

namespace Neurocita.Reactive
{
    public abstract class Log : ILog
    {
        protected Log()
        {
            Source = string.Empty;
        }

        public string Source { get; set; }

        public void Info(string message, params object[] arguments)
        {
            Write(
                new LogEntry()
                {
                    Source = Source,
                    LogEntryType = LogEntryType.Info,
                    Message = string.Format(message, arguments)
                }
            );
        }

        public void Trace(string message, params object[] arguments)
        {
            Write(
                new LogEntry()
                {
                    Source = Source,
                    LogEntryType = LogEntryType.Trace,
                    Message = string.Format(message, arguments)
                }
            );
        }

        public void Warn(string message, params object[] arguments)
        {
            string formattedMessage = string.Format(message, arguments);
            Write(
                new LogEntry()
                {
                    Source = Source,
                    LogEntryType = LogEntryType.Warn,
                    Message = formattedMessage,
                    Exception = new Exception(formattedMessage)
                }
            );
        }

        public void Warn(Exception exception)
        {
            Write(
                new LogEntry()
                {
                    Source = Source,
                    LogEntryType = LogEntryType.Warn,
                    Message = exception.Message,
                    Exception = exception
                }
            );
        }

        public void Warn(Exception exception, string message, params object[] arguments)
        {
            Write(
                new LogEntry()
                {
                    Source = Source,
                    LogEntryType = LogEntryType.Error,
                    Message = string.Format(message, arguments),
                    Exception = exception
                }
            );
        }

        public void Error(string message, params object[] arguments)
        {
            string formattedMessage = string.Format(message, arguments);
            Write(
                new LogEntry()
                {
                    Source = Source,
                    LogEntryType = LogEntryType.Error,
                    Message = formattedMessage,
                    Exception = new Exception(formattedMessage)
                }
            );
        }

        public void Error(Exception exception)
        {
            Write(
                new LogEntry()
                {
                    Source = Source,
                    LogEntryType = LogEntryType.Error,
                    Message = exception.Message,
                    Exception = exception
                }
            );
        }

        public void Error(Exception exception, string message, params object[] arguments)
        {
            Write(
                new LogEntry()
                {
                    Source = Source,
                    LogEntryType = LogEntryType.Error,
                    Message = string.Format(message, arguments),
                    Exception = exception
                }
            );
        }

        public void Entry(LogEntry logEntry)
        {
            Write(logEntry);
        }

        public abstract void Write(LogEntry logEntry);
    }
}
