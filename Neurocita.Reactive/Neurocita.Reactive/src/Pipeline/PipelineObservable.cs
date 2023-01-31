using System;
using System.IO;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive.Pipeline
{
    public static class PipelineObservable
    {
        public static IObservable<IMessage<Stream>> From<TTransport>(TTransport transport, string path)
            where TTransport : ITransport
        {
            return transport.Observe(path);
        }

        public static IObservable<IMessage<Stream>> FromRemote<TTransport>(ITransportFactory<TTransport> transportFactory, string path)
            where TTransport : ITransport
        {
            return transportFactory.Create().Observe(path);
        }

        public static IObservable<IMessage<Stream>> From<TTransport>(Func<TTransport> transportFactory, string path)
            where TTransport : ITransport
        {
            return transportFactory.Invoke().Observe(path);
        }

        public static IObservable<IMessage<Stream>> FromRemote<TState,TTransport>(Func<TState, TTransport> transportFactory, TState state, string path)
            where TTransport : ITransport
        {
            return transportFactory.Invoke(state).Observe(path);
        }
    }
}
