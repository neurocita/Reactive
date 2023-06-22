using System;
using System.IO;

using Neurocita.Reactive.Pipeline;

namespace Neurocita.Reactive.Transport
{
    public interface ITransportMessage : IMessage<Stream>, IDisposable
    {
        
    }
}
