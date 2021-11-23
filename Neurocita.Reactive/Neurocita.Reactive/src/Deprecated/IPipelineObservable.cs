using System;

namespace Neurocita.Reactive
{
    public interface IPipelineObservable<T> : IObservable<T>, IDisposable
    {
        string Node { get; }
    }
}
