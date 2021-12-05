using System;
using System.IO;

namespace Neurocita.Reactive
{
    public static class PipelineObservable
    {
        public static IObservable<IMessage<Stream>> Create(ITransportMessageSource transportMessageSource)
        {
            return transportMessageSource.Messages;
        }

        public static IObservable<IMessage<Stream>> Create(ITransportMessageFactory transportMessageFactory, string source)
        {
            return transportMessageFactory.CreateSource(source).Messages;
        }
    }
}
