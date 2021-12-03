using System;

namespace Neurocita.Reactive
{
    public interface IEndpoint : INode, IDisposable
    {

    }

    public interface IEndpoint<T> : IObservable<T>, IObserver<T>
    {
    }
}
    