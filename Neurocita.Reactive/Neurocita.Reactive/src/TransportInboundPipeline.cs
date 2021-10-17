using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Neurocita.Reactive
{
    internal class TransportInboundPipeline<T> : IDisposableObservable<T>
    {
        private bool disposed = false;
        private readonly IDisposableObservable<ITransportPipelineContext> transportObservable;
        private readonly IDeserializer deserializer;
        private readonly IEnumerable<IPipelineTask<IPipelineContext>> pipelineTasks;

        internal TransportInboundPipeline(ITransportPipeline transportPipeline)
        {
            if (transportPipeline == null)
                throw new ArgumentNullException(nameof(transportPipeline));

            transportObservable = transportPipeline.Transport?.CreateInbound();
            deserializer = transportPipeline.Serializable?.CreateDeserializer();
            pipelineTasks = transportPipeline.InboundTasks ?? new List<IPipelineTask<IPipelineContext>>();

            if (transportObservable == null)
                throw new ArgumentNullException(nameof(transportObservable));
            if (deserializer == null)
                throw new ArgumentNullException(nameof(deserializer));
        }

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
                    ITransportPipelineContext pipelineContext = context;
                    foreach (var pipelineTask in pipelineTasks.Where(task => task is ITransportPipelineTask))
                    {
                        pipelineTask.Run(pipelineContext);
                    }
                    return pipelineContext;
                })
                .Select(context =>
                {
                    T instancce = deserializer.Deserialize<T>(context.Message.Body);
                    return new ObjectPipelineContext(context, PipelineDirection.Inbound, new ObjectMessage<T>(instancce, context.Message.Headers));
                })
                .Select(context =>
                {
                    IObjectPipelineContext pipelineContext = context;
                    foreach (var pipelineTask in pipelineTasks.Where(task => task is IObjectPipelineTask))
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
}
