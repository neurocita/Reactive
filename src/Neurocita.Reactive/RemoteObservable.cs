using System;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive
{
    public static class RemoteObservable
    {
        public static IObservable<TMessage> From<TMessage,TTransport>(TTransport transport, string nodePath)
            where TMessage : ITransportMessage
            where TTransport : ITransport
        {
            return transport.Observe<TMessage>(nodePath);
        }

        public static IObservable<TMessage> From<TMessage,TTransport>(Func<TTransport> transportFactory, string nodePath)
            where TMessage : ITransportMessage
            where TTransport : ITransport
        {
            return transportFactory.Invoke().Observe<TMessage>(nodePath);
        }

        public static IObservable<TMessage> From<TState,TMessage,TTransport>(Func<TState,TTransport> transportFactory, TState state, string nodePath)
            where TMessage : ITransportMessage
            where TTransport : ITransport
        {
            return transportFactory.Invoke(state).Observe<TMessage>(nodePath);
        }
    }
}