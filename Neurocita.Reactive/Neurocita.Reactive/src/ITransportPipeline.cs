using System;
using System.Collections.Generic;
using System.Text;

namespace Neurocita.Reactive
{
    public interface ITransportPipeline
    {
        IRuntimeContext RuntimeContext { get; }
        ISerializable Serializable { get; }
        IEnumerable<Func<ITransportPipelineContext, ITransportPipelineContext>> Interceptors { get; }
        ITransport Transport { get; }
    }
}
