using System.Collections.Generic;
using System.IO;

namespace Neurocita.Reactive
{
    internal class TransportPipelineContext: ITransportPipelineContext
    {
        private readonly IRuntimeContext context;
        private readonly IMessage<Stream> message;

        public TransportPipelineContext(IRuntimeContext context, IMessage<Stream> messge)
        {
            this.context = context;
            this.message = messge;
        }

        public IMessage<Stream> TransportMessage => message;
        public IDictionary<object, object> Properties => context.Properties;
        public ILog Log => context.Log;
    }
}
