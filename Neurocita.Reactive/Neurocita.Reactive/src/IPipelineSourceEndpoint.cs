using System;

namespace Neurocita.Reactive
{
    public interface IPipelineSourceEndpoint : IDisposable
    {
        string Address { get; }
        IObservable<T> GetInstances<T>();
    }
}