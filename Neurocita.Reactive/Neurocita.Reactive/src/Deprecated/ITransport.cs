using System;

namespace Neurocita.Reactive
{
#if _NEVER
    public interface ITransport : IDisposable
    {
        ITransportObservable ObserveFrom(string address);
        ITransportObserver SubscribeTo(string address);
    }
#endif
}
