using System;
using System.Collections.Generic;
using System.IO;

namespace Neurocita.Reactive
{
    internal class TransportPipelineContext: ITransportPipelineContext
    {
        private readonly IRuntimeContext runtimeContext;
        private readonly PipelineDirection direction;
        private readonly IMessage<Stream> message;
        private readonly ISerializer serializer;

        internal TransportPipelineContext(IRuntimeContext runtimeContext, PipelineDirection direction, IMessage<Stream> message, ISerializer serializer)
        {
            this.runtimeContext = runtimeContext ?? throw new ArgumentNullException(nameof(runtimeContext));
            this.direction = direction;
            this.message = message ?? throw new ArgumentNullException(nameof(message));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(TransportPipelineContext.serializer));
        }

        public IDictionary<object, object> Properties => runtimeContext.Properties;
        public ILog Log => runtimeContext.Log;
        public PipelineDirection Direction => direction;
        public IMessage<Stream> Message => message;
        public ISerializer Serializer => serializer;
    }
}
