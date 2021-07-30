using System.Collections.Generic;
using System.IO;

namespace Neurocita.Reactive
{
    internal class PipelineContext : IPipelineContext
    {
        private readonly IRuntimeContext runtimeContext;
        private readonly IMessage<Stream> transitMessage;

        public PipelineContext(IRuntimeContext runtimeContext, IMessage<Stream> transitMessage)
        {
            this.runtimeContext = runtimeContext;
            this.transitMessage = transitMessage;
        }

        public IMessage<Stream> TransportMessage => transitMessage;
        public IDictionary<object, object> Properties => runtimeContext.Properties;
        public ILog Log => runtimeContext.Log;
    }
}
