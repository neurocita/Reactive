using System;
using System.IO;

namespace Neurocita.Reactive.Transport
{
    public interface ITransport : IDisposable
    {
        IObservable<IMessage<Stream>> Observe(string path);
        IDisposable Sink(string path, IObservable<IMessage<Stream>> observable);
        // ToDo: Observe / sink with filters
    }
}