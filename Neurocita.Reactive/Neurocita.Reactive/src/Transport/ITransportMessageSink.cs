using System;
using System.IO;

namespace Neurocita.Reactive.Transport
{
    public interface ITransportMessageSink : INode, IDisposable
    {
        IDisposable Observe(IObservable<IMessage<Stream>> messages);
    }
}
