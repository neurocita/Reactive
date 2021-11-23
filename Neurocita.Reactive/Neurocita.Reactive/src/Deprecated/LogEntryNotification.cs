using System;
using System.Text;

namespace Neurocita.Reactive
{
    public class LogEntryNotification : INotification
    {
        public LogEntryNotification(object sender, LogEntry logEntry)
        {
            Sender = sender;
            LogEntry = logEntry;
        }

        public object Sender { get; }
        public DateTimeOffset Timestamp => DateTimeOffset.UtcNow;
        public LogEntry LogEntry { get; }

        public override string ToString()
        {
            return new StringBuilder()
                .Append($"Sender={Sender}")
                .Append($",Timestamp={Timestamp}")
                .Append($",LogEntry={LogEntry}")
                .ToString();
        }
    }
}
