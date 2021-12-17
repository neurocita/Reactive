using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Neurocita.Reactive.Transport;
using Neurocita.Reactive.Serialization;
using Neurocita.Reactive.Pipeline;

namespace Neurocita.Reactive
{
    internal class SourceEndpoint : ISourceEndpoint
    {
        private bool disposed = false;
        private readonly ITransportMessageSource transportMessageSource;
        private readonly ISerializer serializer;

        public SourceEndpoint(ITransportMessageSource transportMessageSource, ISerializer serializer)
        {
            Util.CheckNullArgument(transportMessageSource);
            Util.CheckNullArgument(serializer);

            this.transportMessageSource = transportMessageSource;
            this.serializer = serializer;
        }

        public IObservable<T> AsObservable<T>() where T : IDataContract
        {
            return AsObservable<T>(new List<IPipelineTask<IPipelineContext>>());
        }

        public IObservable<T> AsObservable<T>(IEnumerable<IPipelineTask<IPipelineContext>> pipelineTasks) where T : IDataContract
        {
            Util.CheckNullArgument(pipelineTasks);

            return disposed
                    ? Observable.Empty<T>()
                    : PipelineObservable
                        .Create(transportMessageSource)
                        .ToPipelineContext()
                        .Tasks(pipelineTasks.Where(task => task is IPipelineTransportTask) as IEnumerable<IPipelineTransportTask>)
                        .Deserialize<T>(serializer)
                        .Tasks(pipelineTasks.Where(task => task is IPipelineObjectTask<T>) as IEnumerable<IPipelineObjectTask<T>>)
                        .ToDataContract<T>();
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                transportMessageSource.Dispose();
                disposed = true;
            }
        }
    }
}
