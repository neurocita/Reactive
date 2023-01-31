using System;
using System.IO;

namespace Neurocita.Reactive.Transport
{
    public interface IInMemoryTransportNode : IDisposable
    {
        string Path { get; }

        IObservable<IMessage<Stream>> Consume();
        IDisposable Produce(IObservable<IMessage<Stream>> source);
    }
}
