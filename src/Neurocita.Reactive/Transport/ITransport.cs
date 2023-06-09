using System;
using System.IO;
using Neurocita.Reactive.Pipeline;

namespace Neurocita.Reactive.Transport
{
    public interface ITransport : IDisposable
    {
        IObservable<ITransportMessage> Observe(string nodePath);
        IDisposable Sink(IObservable<ITransportMessage> observable, string nodePath);
        // ToDo: Observe / sink with filters
    }
}