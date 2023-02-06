using System;
using System.IO;
using Neurocita.Reactive.Pipeline;

namespace Neurocita.Reactive.Transport
{
    public interface ITransport : IDisposable
    {
        IObservable<T> Observe<T>(string path)
            where T : ITransportMessage;
        IDisposable Sink<T>(IObservable<T> observable, string path)
            where T : ITransportMessage;
        // ToDo: Observe / sink with filters
    }
}