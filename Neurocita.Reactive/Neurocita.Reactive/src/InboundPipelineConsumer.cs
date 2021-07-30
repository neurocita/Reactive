using System.Collections.Generic;

namespace Neurocita.Reactive
{
    internal class InboundPipelineConsumer<T> : IPipelineConsumer
    {
        private readonly IDeserializer deserializer;

        public InboundPipelineConsumer(IDeserializer deserializer)
        {
            this.deserializer = deserializer;
        }

        public T Instance { get; private set; }
        public IDictionary<object,object> Headers { get; private set; }

        public void Invoke(IPipelineContext pipelineContext)
        {
            Instance = deserializer.Deserialize<T>(pipelineContext.TransportMessage.Body);
            Headers = pipelineContext.TransportMessage.Headers;
        }
    }
}
