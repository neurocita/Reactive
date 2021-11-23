using System;
using System.Collections.Generic;
using System.Text;

namespace Neurocita.Reactive
{
    [Serializable]
    public class LogEntry
    {
        public DateTimeOffset Timestamp => DateTimeOffset.UtcNow;
        public string Source;
        public LogEntryType LogEntryType = LogEntryType.Trace;
        public string Message;
        public Exception Exception;
        public IDictionary<object, object> Data;

        public override string ToString()
        {
            return new StringBuilder()
                .Append($",Timestamp={Timestamp}")
                .Append(Source == null ? string.Empty : $"Source={Source}")
                .Append($",LogEntryType={LogEntryType}")
                .Append(Message == null ? string.Empty : $",Message={Message}")
                .Append(Exception == null ? string.Empty : $",Exception={Exception}")
                .ToString();
        }
    }
}
