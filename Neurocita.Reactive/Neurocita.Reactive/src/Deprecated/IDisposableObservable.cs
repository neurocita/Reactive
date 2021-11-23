using System;

namespace Neurocita.Reactive
{
    public interface IDisposableObservable<T> : IObservable<T>, IDisposable
    {
    }
}
