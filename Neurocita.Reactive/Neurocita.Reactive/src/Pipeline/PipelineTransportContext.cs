using System;
using System.IO;

namespace Neurocita.Reactive.Pipeline
{
    internal class PipelineTransportContext: IPipelineTransportContext
    {
        private readonly PipelineDirection direction;
        private readonly IMessage<Stream> message;

        internal PipelineTransportContext(PipelineDirection direction, IMessage<Stream> message)
        {
            this.direction = direction;
            this.message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public PipelineDirection Direction => direction;
        public IMessage<Stream> Message => message;
    }
}
