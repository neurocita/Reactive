using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    internal class Pipeline : IPipeline
    {
        public IRuntimeContext RuntimeContext { get; }
        public ISerializer Serializer { get; }
        public ITransport Transport { get; }
        public IEnumerable<IPipelineTask<IPipelineContext>> InboundTasks { get; }
        public IEnumerable<IPipelineTask<IPipelineContext>> OutboundTasks { get; }

        public IPipelineObservable<T> ObserveFrom<T>(string address)
        {
            return new PipelineObservable<T>(this, address);
        }

        public IPipelineSubscriber SubscribeTo<T>(string address, IObservable<T> observable)
        {
            return new PipelineSubscriber<T>(this, address, observable);
        }
    }
}
