using System;
using System.IO;

namespace Neurocita.Reactive
{
    public interface ITransportMessageSink : IDisposable
    {
        string Node { get; }
        IDisposable Observe(IObservable<IMessage<Stream>> messages);
    }
}
