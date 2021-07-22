using System;

namespace Neurocita.Reactive
{
    public interface IEndpoint<T> : IObservable<T>, IObserver<T>, IDisposable
    {
    }
}
