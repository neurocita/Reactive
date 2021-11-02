using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public interface IPipeline
    {
        IRuntimeContext RuntimeContext { get; }
        ISerializer Serializer { get; }
        ITransport Transport { get; }
        IEnumerable<IPipelineTask<IPipelineContext>> InboundTasks { get; }
        IEnumerable<IPipelineTask<IPipelineContext>> OutboundTasks { get; }
        IPipelineObservable<T> ObserveFrom<T>(string address);
        IPipelineSubscriber SubscribeTo<T>(string address, IObservable<T> observable);
    }
}
