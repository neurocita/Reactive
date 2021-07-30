using System.Collections.Generic;
using System.IO;

namespace Neurocita.Reactive
{
    internal class TransportMessage : IMessage<Stream>
    {
        public TransportMessage(Stream body, IDictionary<object, object> headers = null)
        {
            Body = body ?? new MemoryStream();
            Headers = headers ?? new Dictionary<object, object>();
        }

        public IDictionary<object, object> Headers { get; }
        public Stream Body { get; }
    }
}
