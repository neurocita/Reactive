using System;
using System.IO;

namespace Neurocita.Reactive
{
    public interface ITransportMessageSink : INode, IDisposable
    {
        IDisposable Observe(IObservable<IMessage<Stream>> messages);
    }
}
