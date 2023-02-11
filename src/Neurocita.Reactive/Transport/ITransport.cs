using System;
using System.IO;
using Neurocita.Reactive.Pipeline;

namespace Neurocita.Reactive.Transport
{
    public interface ITransport : IDisposable
    {
        IObservable<T> Observe<T>(string nodePath)
            where T : ITransportMessage;
        IDisposable Sink<T>(IObservable<T> observable, string nodePath)
            where T : ITransportMessage;
        // ToDo: Observe / sink with filters
    }
}