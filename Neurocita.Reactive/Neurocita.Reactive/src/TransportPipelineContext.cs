using System.IO;

namespace Neurocita.Reactive
{
    internal class TransportPipelineContext: ITransportPipelineContext
    {
        private readonly IRuntimeContext context;
        private readonly IMessage<Stream> message;

        public TransportPipelineContext(IRuntimeContext context, IMessage<Stream> message)
        {
            this.context = context;
            this.message = message;
        }

        public IRuntimeContext Runtime => context;
        public IMessage<Stream> Message => message;
    }
}
