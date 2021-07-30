using System;

namespace Neurocita.Reactive
{
    public interface IObservableEndpoint<T> : IEndpoint, IObservable<T>, IObserver<T>
    {
    }
}
