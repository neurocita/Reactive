using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public class ObjectMessage<TDataContract> : IMessage<TDataContract>
        where TDataContract : IDataContract
    {
        public ObjectMessage(TDataContract body)
            : this(body, null)
        {

        }
        public ObjectMessage(TDataContract body, IDictionary<string, object> headers = null)
        {
            if (body == null)
                throw new ArgumentNullException(nameof(body));
            Body = body;
            Headers = headers ?? new Dictionary<string, object>();
        }

        public IDictionary<string, object> Headers { get; }
        public TDataContract Body { get; }
    }
}
