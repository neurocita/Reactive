using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    internal class Message<T> : IMessage<T>
    {
        public Message(T body)
            : this(body, null)
        {

        }
        public Message(T body, IDictionary<string, object> headers = null)
        {
            //if (body == null)
            //    throw new ArgumentNullException(nameof(body));
            Body = body;
            Headers = headers ?? new Dictionary<string, object>();
            if (!Headers.ContainsKey(MessageHeaders.ContentSourceType))
                Headers[MessageHeaders.ContentSourceType] = typeof(T);
            if (!Headers.ContainsKey(MessageHeaders.CreationTime))
                Headers[MessageHeaders.CreationTime] = DateTimeOffset.UtcNow;
            //if (!Headers.ContainsKey(MessageHeaders.ExpiryTime))
            //    Headers[MessageHeaders.ExpiryTime] = ((DateTimeOffset) Headers[MessageHeaders.CreationTime]).AddDays(14);
        }

        public IDictionary<string, object> Headers { get; }
        public T Body { get; }
    }
}
