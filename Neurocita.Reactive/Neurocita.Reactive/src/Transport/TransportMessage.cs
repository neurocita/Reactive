using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Neurocita.Reactive.Transport
{
    public class TransportMessage : IMessage<Stream>, ICloneable
    {
        public TransportMessage(Stream body, IDictionary<string, object> headers = null)
        {
            Body = body ?? new MemoryStream();
            Headers = headers ?? new Dictionary<string, object>();
        }

        public TransportMessage(byte[] buffer, IDictionary<string, object> headers = null)
        {
            Body = new MemoryStream(buffer);
            Headers = headers ?? new Dictionary<string, object>();
        }

        // Null-Message, used to transfer OnCompleted event
        public TransportMessage(IDictionary<string, object> headers = null)
        {
            Headers = headers ?? new Dictionary<string, object>();
        }

        public IDictionary<string, object> Headers { get; }
        public Stream Body { get; }

        public object Clone()
        {
            // https://dotnetcoretutorials.com/2020/09/09/cloning-objects-in-c-and-net-core/
            /*
            TransportMessage transportMessage = new TransportMessage();
            Body.CopyTo(transportMessage.Body);
            foreach (var header in Headers)
            {
                 transportMessage.Headers.Add(header.Key?.Clone() as string
                                                , header.Value is ICloneable
                                                    ? (header.Value as ICloneable)?.Clone()
                                                    : header.Value);
            }
            return transportMessage;
            */
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);
            }
        }
    }
}
