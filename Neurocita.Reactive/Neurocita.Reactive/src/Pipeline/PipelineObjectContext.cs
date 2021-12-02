using System.Collections.Generic;

namespace Neurocita.Reactive
{
    internal class PipelineObjectContext<TDataContract> : IPipelineObjectContext<TDataContract>
        where TDataContract : IDataContract
    {
        private readonly PipelineDirection direction;
        private readonly IMessage<TDataContract> message;
        /*
        internal PipelineObjectContext(IPipelineTransportContext transportPipelineContext, TDataContract instance, IDictionary<string, object> headers = null)
            : this(transportPipelineContext, transportPipelineContext.Direction, instance, headers)
        {

        }

        internal PipelineObjectContext(IPipelineTransportContext transportPipelineContext, IMessage<TDataContract> message)
            : this(transportPipelineContext, transportPipelineContext.Direction, message)
        {

        }
        */
        internal PipelineObjectContext(PipelineDirection direction, TDataContract instance, IDictionary<string, object> headers = null)
            : this(direction, new ObjectMessage<TDataContract>(instance, headers))
        {

        }

        internal PipelineObjectContext(PipelineDirection direction, IMessage<TDataContract> message)
        {
            this.direction = direction;
            this.message = message;
        }

        public PipelineDirection Direction => direction;
        public IMessage<TDataContract> Message => message;

    }
}
