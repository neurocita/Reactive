using System.Collections.Generic;
using System.IO;

namespace Neurocita.Reactive
{
    internal class OutboundPipelineProducer<T> : IPipelineProducer
    {
        private readonly IRuntimeContext runtimeContext;
        private readonly T instance;
        private readonly ISerializer serializer;
        private readonly IDictionary<object, object> headers;

        public OutboundPipelineProducer(IRuntimeContext runtimeContext, T instance, ISerializer serializer, IDictionary<object,object> headers = null)
        {
            this.runtimeContext = runtimeContext;
            this.instance = instance;
            this.serializer = serializer;
            this.headers = headers ?? new Dictionary<object, object>();
        }

        public IPipelineContext Invoke()
        {
            Stream stream = serializer.Serialize(instance);
            IMessage<Stream> message = new TransportMessage(stream, headers);
            return new PipelineContext(runtimeContext, message);
        }
    }
}
