using System;

namespace Neurocita.Reactive
{
    public interface ITransport : IDisposable
    {
        ITransportObservable ObserveFrom(string address);
        ITransportObserver SubscribeTo(string address);
    }
}
