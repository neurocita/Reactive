using System;
using System.Collections.Generic;
using Neurocita.Reactive.Pipeline;

namespace Neurocita.Reactive.Configuration
{
    public interface ICanAddSourceOrSink
    {
        ICanAddEndpointOrBuild WithSource<TDataContract>() where TDataContract : IDataContract;
        ICanAddEndpointOrBuild WithSource<TDataContract>(IEnumerable<IPipelineTask<IPipelineContext>> tasks) where TDataContract :IDataContract;
        ICanAddEndpointOrBuild WithSource<TDataContract>(IEnumerable<Action<IPipelineContext>> tasks) where TDataContract : IDataContract;
        ICanAddEndpointOrBuild WithSink<TDataContract>(IObservable<TDataContract> source) where TDataContract : IDataContract;
        ICanAddEndpointOrBuild WithSink<TDataContract>(IObservable<TDataContract> source, IEnumerable<IPipelineTask<IPipelineContext>> tasks) where TDataContract : IDataContract;
        ICanAddEndpointOrBuild WithSink<TDataContract>(IObservable<TDataContract> source, IEnumerable<Action<IPipelineContext>> tasks) where TDataContract : IDataContract;
    }
}
