using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Neurocita.Reactive
{
#if _NEVER
    internal class PipelineObservable<T> : IPipelineObservable<T>
    {
        private bool disposed = false;
        private readonly string node;
        private readonly IDisposableObservable<IPipelineTransportContext> transportObservable;
        private readonly ISerializer serializer;
        private readonly IEnumerable<IPipelineTask<IPipelineContext>> pipelineTasks;

        internal PipelineObservable(IPipeline transportPipeline, string address)
        {
            if (transportPipeline == null)
                throw new ArgumentNullException(nameof(transportPipeline));
            if (string.IsNullOrEmpty(address))
                throw new ArgumentNullException(nameof(address));

            this.node = address;
            transportObservable = transportPipeline.Transport?.ObserveFrom(address);
            serializer = transportPipeline.Serializer;
            pipelineTasks = transportPipeline.InboundTasks ?? new List<IPipelineTask<IPipelineContext>>();

            if (transportObservable == null)
                throw new ArgumentNullException(nameof(transportObservable));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));
        }

        public string Node => node;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (disposed)
                return Disposable.Empty;

            return transportObservable
                .Do(context =>
                {
                    if (context.Direction != PipelineDirection.Inbound)
                        throw new ArgumentOutOfRangeException(nameof(context.Direction));
                })
                .Select(context =>
                {
                    IPipelineTransportContext pipelineContext = context;
                    foreach (var pipelineTask in pipelineTasks.Where(task => task is IPipelineTransportTask))
                    {
                        pipelineTask.Run(pipelineContext);
                    }
                    return pipelineContext;
                })
                .Select(context =>
                {
                    T instancce = serializer.Deserialize<T>(context.Message.Body);
                    return new PipelineObjectContext(context, PipelineDirection.Inbound, new ObjectMessage<T>(instancce, context.Message.Headers));
                })
                .Select(context =>
                {
                    IPipelineObjectContext pipelineContext = context;
                    foreach (var pipelineTask in pipelineTasks.Where(task => task is IPipelineObjectTask))
                    {
                        pipelineTask.Run(pipelineContext);
                    }
                    return pipelineContext;
                })
                .Select(context => (T) context.Message.Body)
                .Subscribe(observer);
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            transportObservable?.Dispose();

            disposed = true;
        }
    }
#endif
}
