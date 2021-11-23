using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
#if _NEVER
    internal class Pipeline : IPipeline
    {
        public IRuntimeContext RuntimeContext { get; }
        public ISerializer Serializer { get; }
        public ITransport Transport { get; }
        public IEnumerable<IPipelineTask<IPipelineContext>> InboundTasks { get; }
        public IEnumerable<IPipelineTask<IPipelineContext>> OutboundTasks { get; }

        public IPipelineObservable<T> ObserveFrom<T>(string node)
        {
            return new PipelineObservable<T>(this, node);
        }

        public IPipelineSubscriber SubscribeTo<T>(string node, IObservable<T> observable)
        {
            return new PipelineSubscriber<T>(this, node, observable);
        }
    }
#endif
}
