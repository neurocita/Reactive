using System;

namespace Neurocita.Reactive.Pipes
{
    public interface IDisposableObservable<T> : IObservable<T>, IDisposable
    {
    }
}
