using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.IO;
using System.Reactive.Disposables;

namespace Neurocita.Reactive
{
#if _NEVER
    internal class PipelineSubscriber<T> : IPipelineSubscriber
    {
        private bool disposed = false;
        private readonly string node;
        private readonly IRuntimeContext runtimeContext;
        private readonly ITransportObserver transportObserver;
        private readonly ISerializer serializer;
        private readonly IEnumerable<IPipelineTask<IPipelineContext>> pipelineTasks;
        private CompositeDisposable disposable = new CompositeDisposable();

        internal PipelineSubscriber(IPipeline pipeline, string node, IObservable<T> observable, IDictionary<string, object> messageHeaders = null)
        {
            if (pipeline == null)
                throw new ArgumentNullException(nameof(pipeline));
            if (string.IsNullOrEmpty(node))
                throw new ArgumentNullException(nameof(node));

            this.node = node;
            runtimeContext = pipeline.RuntimeContext;
            transportObserver = pipeline.Transport?.SubscribeTo(node);
            serializer = pipeline.Serializer;
            pipelineTasks = pipeline.OutboundTasks ?? new List<IPipelineTask<IPipelineContext>>();

            if (runtimeContext == null)
                throw new ArgumentNullException(nameof(runtimeContext));
            if (transportObserver == null)
                throw new ArgumentNullException(nameof(transportObserver));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            disposable.Add(transportObserver);
            disposable.Add(observable?
                .Select(instance =>
                {
                    IDictionary<string, object> headers = messageHeaders ?? new Dictionary<string, object>();
                    if (!headers.ContainsKey(MessageHeaders.ContentType) && !string.IsNullOrWhiteSpace(pipeline.Serializer.ContentType))
                        headers.Add(MessageHeaders.ContentType, pipeline.Serializer.ContentType);
                    if (!headers.ContainsKey(MessageHeaders.QualifiedTypeName))
                        headers.Add(MessageHeaders.QualifiedTypeName, typeof(T).FullName);
                    if (!headers.ContainsKey(MessageHeaders.CreationTime))
                        headers.Add(MessageHeaders.CreationTime, DateTimeOffset.UtcNow);
                    var message = new ObjectMessage<T>(instance, headers);
                    return new PipelineObjectContext(runtimeContext, PipelineDirection.Outbound, message) as IPipelineObjectContext;
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
                .Select(context =>
                {
                    Stream stream = serializer.Serialize((T) context.Message.Body);
                    TransportMessage transportMessage = new TransportMessage(stream, context.Message.Headers);
                    return new PipelineTransportContext(context, context.Direction, transportMessage, pipeline.Serializer) as IPipelineTransportContext;
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
                .Subscribe(transportObserver)
            );
        }

        public string Node => node;

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            disposable?.Dispose();

            disposed = true;
        }
    }
#endif
}
