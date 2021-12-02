using System;

namespace Neurocita.Reactive
{
    public interface IEndpoint : INode
    {

    }

    public interface IEndpoint<T> : IObservable<T>, IObserver<T>
    {
    }
}
    