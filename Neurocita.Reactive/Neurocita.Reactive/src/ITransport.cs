using System;

namespace Neurocita.Reactive
{
    public interface ITransport : IDisposable
    {
        ITransportObservable CreateInbound();
        ITransportObserver CreateOutbound();
    }
}
