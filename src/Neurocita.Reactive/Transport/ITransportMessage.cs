using System.IO;
using System;
using Neurocita.Reactive.Pipeline;

namespace Neurocita.Reactive.Transport
{
    public interface ITransportMessage : IMessage<Stream>, IDisposable
    {
        
    }
}
