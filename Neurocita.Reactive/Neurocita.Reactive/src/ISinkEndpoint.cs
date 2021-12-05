using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public interface ISinkEndpoint : IDisposable
    {
        IDisposable From<T>(IObservable<T> observable) where T : IDataContract;
        IDisposable From<T>(IObservable<T> observable, IEnumerable<IPipelineTask<IPipelineContext>> pipelineTasks) where T : IDataContract;
    }
}
