using System;
using System.Runtime.Serialization;

namespace Neurocita.Reactive.Logging
{
    [Serializable]
    [DataContract]
    public class LogEvent
    {
        private readonly DateTimeOffset timestamp = DateTimeOffset.Now;
        private readonly int threadId = Environment.CurrentManagedThreadId;
        private readonly int processId = System.Diagnostics.Process.GetCurrentProcess().Id;
        private readonly string sourceTypeFullName;
        private readonly LogContext context;
        private readonly LogLevel logLevel;
        private readonly string message;
        private readonly Exception exception;

        public LogEvent(object instance, LogContext context, LogLevel logLevel, string message)
        {
            sourceTypeFullName = instance.GetType().FullName;
            this.logLevel = logLevel;
            this.context = context;
            this.message = message;
        }

        public LogEvent(object instance, LogContext context, LogLevel logLevel, string message, Exception exception)
            : this(instance, context, logLevel, message)
        {
            this.exception = exception;
        }

        [DataMember]
        public DateTimeOffset Timestamp => timestamp;
        [DataMember]
        public int ThreadId => threadId;
        [DataMember]
        public int ProcessId => processId;
        [DataMember]
        public string SourceTypeFullName => sourceTypeFullName;
        [DataMember]
        public LogLevel LogLevel => logLevel;
        [DataMember]
        public LogContext Context => context;
        [DataMember]
        public string Message => message;
        [DataMember]
        public Exception Exception => exception;
    }
}
