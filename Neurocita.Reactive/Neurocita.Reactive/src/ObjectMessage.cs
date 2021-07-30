using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public class ObjectMessage<T> : IMessage<T>
    {
        public ObjectMessage(T body)
            : this(null, body)
        {

        }
        public ObjectMessage(IDictionary<object, object> headers, T body)
        {
            if (body == null)
                throw new ArgumentNullException(nameof(body));
            Headers = headers ?? new Dictionary<object, object>();
            Body = body;
        }

        public IDictionary<object, object> Headers { get; }
        public T Body { get; }
    }
}
