using System;

namespace Neurocita.Reactive
{
    public interface ISourceEndpoint : IEndpoint
    {
        IObservable<T> AsObservable<T>() where T : IDataContract;
    }
}
