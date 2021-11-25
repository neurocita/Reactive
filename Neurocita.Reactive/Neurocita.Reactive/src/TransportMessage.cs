using System.Collections.Generic;
using System.IO;

namespace Neurocita.Reactive
{
    public class TransportMessage : IMessage<Stream>
    {
        public TransportMessage(Stream body, IDictionary<string, object> headers = null)
        {
            Body = body ?? new MemoryStream();
            Headers = headers ?? new Dictionary<string, object>();
        }

        public IDictionary<string, object> Headers { get; }
        public Stream Body { get; }
    }
}
