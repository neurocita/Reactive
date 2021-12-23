using System;
using System.Collections.Generic;
using Neurocita.Reactive.Pipeline;

namespace Neurocita.Reactive.Configuration
{
    public interface ICanAddSourceOrSink
    {
        ICanAddEndpointOrCreate WithSource<TDataContract>() where TDataContract : IDataContract;
        ICanAddEndpointOrCreate WithSource<TDataContract>(IEnumerable<IPipelineTask<IPipelineContext>> tasks) where TDataContract :IDataContract;
        ICanAddEndpointOrCreate WithSource<TDataContract>(IEnumerable<Action<IPipelineContext>> tasks) where TDataContract : IDataContract;
        ICanAddEndpointOrCreate WithSink<TDataContract>(IObservable<TDataContract> source) where TDataContract : IDataContract;
        ICanAddEndpointOrCreate WithSink<TDataContract>(IObservable<TDataContract> source, IEnumerable<IPipelineTask<IPipelineContext>> tasks) where TDataContract : IDataContract;
        ICanAddEndpointOrCreate WithSink<TDataContract>(IObservable<TDataContract> source, IEnumerable<Action<IPipelineContext>> tasks) where TDataContract : IDataContract;
    }
}
