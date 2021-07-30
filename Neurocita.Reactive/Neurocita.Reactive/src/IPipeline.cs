using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public interface IPipeline<T>
    {
        IPipelineProducer Producer { get; }
        IEnumerable<IPipelineStep> Steps { get; }
        IPipelineConsumer Consumer { get; }
    }
}
