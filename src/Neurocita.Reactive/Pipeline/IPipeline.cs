using System;
using Neurocita.Reactive.Transport;
using Neurocita.Reactive.Serialization;

namespace Neurocita.Reactive.Pipeline
{   
    public interface IPipeline : IDisposable
    {
        ITransport Transport { get; }
        ISerializer Serializer { get; }

        IDisposable Execute<T>(string nodePath, IObservable<T> source);
        IObservable<T> Execute<T>(string nodePath);
    }
}