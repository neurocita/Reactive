using System;
using System.IO;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive.Pipeline
{
    public static class PipelineObservable
    {
        public static IObservable<IMessage<Stream>> Create(ITransportMessageSource transportMessageSource)
        {
            return transportMessageSource.Messages;
        }

        public static IObservable<IMessage<Stream>> Create(ITransport transport, string nodePath)
        {
            return transport.CreateSource(nodePath).Messages;
        }

        public static IObservable<IMessage<Stream>> Create(ITransportFactory transportFactory, string nodePath)
        {
            return transportFactory.Create().CreateSource(nodePath).Messages;
        }
    }
}
