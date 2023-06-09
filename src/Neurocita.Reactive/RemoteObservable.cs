using System;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive
{
    public static class RemoteObservable
    {
        public static IObservable<ITransportMessage> From(ITransport transport, string nodePath)
        {
            return transport.Observe(nodePath);
        }

        public static IObservable<ITransportMessage> From(Func<ITransport> transportFactory, string nodePath)
        {
            return transportFactory.Invoke().Observe(nodePath);
        }

        public static IObservable<ITransportMessage> From<TState>(Func<TState,ITransport> transportFactory, TState state, string nodePath)
        {
            return transportFactory.Invoke(state).Observe(nodePath);
        }
    }
}