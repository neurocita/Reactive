using System.Collections.Generic;

namespace Neurocita.Reactive
{
    internal class ObjectPipelineContext : IObjectPipelineContext
    {
        private readonly IRuntimeContext runtimeContext;
        private readonly PipelineDirection direction;
        private readonly IMessage<object> message;

        internal ObjectPipelineContext(ITransportPipelineContext transportPipelineContext, object instance, IDictionary<string, object> headers = null)
            : this(transportPipelineContext, transportPipelineContext.Direction, instance, headers)
        {

        }

        internal ObjectPipelineContext(ITransportPipelineContext transportPipelineContext, IMessage<object> message)
            : this(transportPipelineContext, transportPipelineContext.Direction, message)
        {

        }

        internal ObjectPipelineContext(IRuntimeContext runtimeContext, PipelineDirection direction, object instance, IDictionary<string, object> headers = null)
            : this(runtimeContext, direction, new ObjectMessage<object>(instance, headers))
        {

        }

        internal ObjectPipelineContext(IRuntimeContext runtimeContext, PipelineDirection direction, IMessage<object> message)
        {
            this.runtimeContext = runtimeContext;
            this.direction = direction;
            this.message = message;
        }

        public IDictionary<object, object> Properties => runtimeContext.Properties;
        public ILog Log => runtimeContext.Log;
        public PipelineDirection Direction => direction;
        public IMessage<object> Message => message;

    }
}
