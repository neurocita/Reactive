using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.IO;

namespace Neurocita.Reactive
{
    internal class TransportOutboundPipeline<T> : IDisposable
    {
        private bool disposed = false;
        private readonly IRuntimeContext runtimeContext;
        private readonly ITransportObserver transportObserver;
        private readonly ISerializer serializer;
        private readonly IEnumerable<IPipelineTask<IPipelineContext>> pipelineTasks;
        private IDisposable disposable;

        internal TransportOutboundPipeline(ITransportPipeline transportPipeline, IObservable<T> observable, IDictionary<string, object> messageHeaders = null)
        {
            if (transportPipeline == null)
                throw new ArgumentNullException(nameof(transportPipeline));

            runtimeContext = transportPipeline.RuntimeContext;
            transportObserver = transportPipeline.Transport?.CreateOutbound();
            serializer = transportPipeline.Serializable?.CreateSerializer();
            pipelineTasks = transportPipeline.OutboundTasks ?? new List<IPipelineTask<IPipelineContext>>();

            if (runtimeContext == null)
                throw new ArgumentNullException(nameof(runtimeContext));
            if (transportObserver == null)
                throw new ArgumentNullException(nameof(transportObserver));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            disposable = observable
                .Select(instance =>
                {
                    IDictionary<string, object> headers = messageHeaders ?? new Dictionary<string, object>();
                    if (!headers.ContainsKey(MessageHeaders.ContentType) && !string.IsNullOrWhiteSpace(transportPipeline.Serializable.ContentType))
                        headers.Add(MessageHeaders.ContentType, transportPipeline.Serializable.ContentType);
                    if (!headers.ContainsKey(MessageHeaders.QualifiedTypeName))
                        headers.Add(MessageHeaders.QualifiedTypeName, typeof(T).FullName);
                    if (!headers.ContainsKey(MessageHeaders.CreationTime))
                        headers.Add(MessageHeaders.CreationTime, DateTimeOffset.UtcNow);
                    var message = new ObjectMessage<T>(instance, headers);
                    return new ObjectPipelineContext(runtimeContext, PipelineDirection.Outbound, message);
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
                .Select(context =>
                {
                    Stream stream = serializer.Serialize((T) context.Message.Body);
                    TransportMessage transportMessage = new TransportMessage(stream, context.Message.Headers);
                    return new TransportPipelineContext(context, context.Direction, transportMessage, transportPipeline.Serializable);
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
                .Subscribe(transportObserver);
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            disposable?.Dispose();
            transportObserver?.Dispose();

            disposed = true;
        }
    }
}
