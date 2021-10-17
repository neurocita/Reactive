using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public interface ITransportPipeline
    {
        IRuntimeContext RuntimeContext { get; }
        ISerialization Serializable { get; }
        ITransport Transport { get; }
        IEnumerable<IPipelineTask<IPipelineContext>> InboundTasks { get; }
        IEnumerable<IPipelineTask<IPipelineContext>> OutboundTasks { get; }
        IDisposableObservable<T> CreateInbound<T>();
        IDisposable CreateOutbound<T>(IObservable<T> observable);
    }
}
