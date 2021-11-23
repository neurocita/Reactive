using System;

namespace Neurocita.Reactive
{
    public interface IPipelineSourceEndpoint<T> : IDisposable
    {
        string Node { get; }
        IObservable<T> Data { get; }
    }
}