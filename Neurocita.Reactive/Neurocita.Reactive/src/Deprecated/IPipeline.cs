using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
#if _NEVER
    public interface IPipeline
    {
        IRuntimeContext RuntimeContext { get; }
        ISerializer Serializer { get; }
        ITransport Transport { get; }
        IEnumerable<IPipelineTask<IPipelineContext>> InboundTasks { get; }
        IEnumerable<IPipelineTask<IPipelineContext>> OutboundTasks { get; }
        IPipelineObservable<T> ObserveFrom<T>(string node);
        IPipelineSubscriber SubscribeTo<T>(string node, IObservable<T> observable);
    }
#endif
}
