using System;

namespace Neurocita.Reactive
{
    public interface ISinkEndpoint : IEndpoint
    {
        IDisposable From<T>(IObservable<T> observable) where T : IDataContract;
    }
}
