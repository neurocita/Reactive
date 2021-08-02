using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public class ObjectMessage<T> : IMessage<T>
    {
        public ObjectMessage(T body)
        {

        }
        public ObjectMessage(T body, IDictionary<object, object> headers = null)
        {
            if (body == null)
                throw new ArgumentNullException(nameof(body));
            Body = body;
            Headers = headers ?? new Dictionary<object, object>();
        }

        public IDictionary<object, object> Headers { get; }
        public T Body { get; }
    }
}
