using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    internal class TransportPipeline : ITransportPipeline
    {
        public IRuntimeContext RuntimeContext { get; }
        public ISerialization Serializable { get; }
        public ITransport Transport { get; }
        public IEnumerable<IPipelineTask<IPipelineContext>> InboundTasks { get; }
        public IEnumerable<IPipelineTask<IPipelineContext>> OutboundTasks { get; }

        public IDisposableObservable<T> CreateInbound<T>()
        {
            return new TransportInboundPipeline<T>(this);
        }

        public IDisposable CreateOutbound<T>(IObservable<T> observable)
        {
            return new TransportOutboundPipeline<T>(this, observable);
        }
    }
}
