using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public class ObjectMessage<T> : IMessage<T>
    {
        public ObjectMessage(T body)
            : this(body, null)
        {

        }
        public ObjectMessage(T body, IDictionary<string, object> headers = null)
        {
            if (body == null)
                throw new ArgumentNullException(nameof(body));
            Body = body;
            Headers = headers ?? new Dictionary<string, object>();
        }

        public IDictionary<string, object> Headers { get; }
        public T Body { get; }
    }
}
