using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public interface ISourceEndpoint : IDisposable
    {
        IObservable<T> AsObservable<T>() where T : IDataContract;
        IObservable<T> AsObservable<T>(IEnumerable<IPipelineTask<IPipelineContext>> pipelineTasks) where T : IDataContract;
    }
}
