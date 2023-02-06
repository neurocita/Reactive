using System;
using System.Collections.Generic;

namespace Neurocita.Reactive.Pipeline
{
    internal class Message<T> : IMessage<T>
    {
        public Message(T body, IDictionary<string,object> headers = null)
        {
            Body = body;
            Headers = headers ?? new Dictionary<string,object>();

            if (!Headers.ContainsKey(MessageHeaders.ContentTypeFullName))
                Headers[MessageHeaders.ContentTypeFullName] = typeof(T).FullName;
            if (!Headers.ContainsKey(MessageHeaders.CreationTime))
                Headers[MessageHeaders.CreationTime] = DateTimeOffset.UtcNow;
        }

        public Message(Message<T> message)
        {
            Body = message.Body;
            Headers = message.Headers;
        }

        public IDictionary<string, object> Headers { get; private set; }
        public T Body { get; private set; }
    }
}