using System;
using System.Collections.Generic;
using Neurocita.Reactive.Pipeline;

namespace Neurocita.Reactive
{
    public interface ISourceEndpoint
    {
        IObservable<T> AsObservable<T>() where T : IDataContract;
        IObservable<T> AsObservable<T>(IEnumerable<IPipelineTask<IPipelineContext>> pipelineTasks) where T : IDataContract;
    }
}
