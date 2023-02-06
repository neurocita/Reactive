using System;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive
{
    public static class ObservableExtensions
    {
        public static IDisposable To<T,U>(this IObservable<T> transportMessages, U transport, string nodePath)
            where T : ITransportMessage
            where U : ITransport
        {
            return transport.Sink(transportMessages, nodePath);
        }

        public static IDisposable To<T,U>(this IObservable<T> transportMessages, Func<U> transportFactory, string nodePath)
        where T : ITransportMessage
        where U : ITransport
        {
            return transportFactory.Invoke().Sink(transportMessages, nodePath);
        }

        public static IDisposable To<S,T,U>(this IObservable<T> transportMessages, Func<S,U> transportFactory, S state, string nodePath)
            where T : ITransportMessage
            where U : ITransport
        {
            return transportFactory.Invoke(state).Sink(transportMessages, nodePath);
        }
    }
}
