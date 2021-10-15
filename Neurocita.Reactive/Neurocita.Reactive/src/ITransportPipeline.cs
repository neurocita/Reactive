using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public interface ITransportPipeline
    {
        IRuntimeContext RuntimeContext { get; }
        ISerialization Serializable { get; }
        ITransport Transport { get; }
        IEnumerable<Func<ITransportPipelineContext, ITransportPipelineContext>> InboundInterceptors { get; }
        IEnumerable<Func<ITransportPipelineContext, ITransportPipelineContext>> OutboundInterceptors { get; }
        IDisposableObservable<T> CreateInbound<T>();
        IDisposable CreateOutbound<T>(IObservable<T> observable);
    }
}
