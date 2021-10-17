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
        private readonly ISerialization serialization;

        internal TransportPipelineContext(IRuntimeContext runtimeContext, PipelineDirection direction, IMessage<Stream> message, ISerialization serialization)
        {
            this.runtimeContext = runtimeContext ?? throw new ArgumentNullException(nameof(runtimeContext));
            this.direction = direction;
            this.message = message ?? throw new ArgumentNullException(nameof(message));
            this.serialization = serialization ?? throw new ArgumentNullException(nameof(serialization));
        }

        public IDictionary<object, object> Properties => runtimeContext.Properties;
        public ILog Log => runtimeContext.Log;
        public PipelineDirection Direction => direction;
        public IMessage<Stream> Message => message;
        public ISerialization Serialization => serialization;
    }
}
