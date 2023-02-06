using System;
using System.Collections.Generic;
using System.IO;
using Neurocita.Reactive.Utilities;

namespace Neurocita.Reactive.Transport
{
    internal class TransportMessage : ITransportMessage
    {
        private bool disposedValue;

        public TransportMessage()
        {
            Body = null;
            Headers = new Dictionary<string,object>();
        }

        public TransportMessage(Stream body, IDictionary<string, object> headers = null)
        {
            Body = body;
            Headers = headers ?? new Dictionary<string, object>();
        }

        public TransportMessage(TransportMessage transportMessage)
        {
            //transportMessage.Body?.CopyTo(Body);
            //Headers = transportMessage.Headers.DeepCopy();
            Body = transportMessage.Body;
            Headers = transportMessage.Headers;
        }

        public IDictionary<string, object> Headers { get; }

        public Stream Body { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Body?.Dispose();
                }

                 disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
