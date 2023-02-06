using System;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive
{
    public static class RemoteObservable
    {
        public static IObservable<T> From<T,U>(U transport, string nodePath)
            where T : ITransportMessage
            where U : ITransport
        {
            return transport.Observe<T>(nodePath);
        }

        public static IObservable<T> From<T,U>(Func<U> transportFactory, string nodePath)
            where T : ITransportMessage
            where U : ITransport
        {
            return transportFactory.Invoke().Observe<T>(nodePath);
        }

        public static IObservable<T> From<S,T,U>(Func<S,U> transportFactory, S state, string nodePath)
            where T : ITransportMessage
            where U : ITransport
        {
            return transportFactory.Invoke(state).Observe<T>(nodePath);
        }
    }
}