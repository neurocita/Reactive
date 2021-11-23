using System;

namespace Neurocita.Reactive
{
    public interface IEndpoint
    {

    }

    public interface IEndpoint<T> : IObservable<T>, IObserver<T>
    {
    }
}
