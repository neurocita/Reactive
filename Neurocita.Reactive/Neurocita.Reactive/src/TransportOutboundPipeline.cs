using System;
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
        private readonly IEnumerable<Func<ITransportPipelineContext, ITransportPipelineContext>> interceptors;
        private IDisposable disposable;

        internal TransportOutboundPipeline(ITransportPipeline transportPipeline, IObservable<T> observable, IDictionary<string, object> messageHeaders = null)
        {
            if (transportPipeline == null)
                throw new ArgumentNullException(nameof(transportPipeline));

            runtimeContext = transportPipeline.RuntimeContext;
            transportObserver = transportPipeline.Transport?.CreateOutbound();
            serializer = transportPipeline.Serializable?.CreateSerializer();
            interceptors = transportPipeline.OutboundInterceptors ?? new List<Func<ITransportPipelineContext, ITransportPipelineContext>>();

            if (runtimeContext == null)
                throw new ArgumentNullException(nameof(runtimeContext));
            if (transportObserver == null)
                throw new ArgumentNullException(nameof(transportObserver));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            disposable = observable
                .Select(
                    state =>
                    {
                        Stream stream = serializer.Serialize(state);
                        IDictionary<string, object> headers = messageHeaders ?? new Dictionary<string, object>();
                        if (!headers.ContainsKey(MessageHeaders.ContentType) && !string.IsNullOrWhiteSpace(transportPipeline.Serializable.ContentType))
                            headers.Add(MessageHeaders.ContentType, transportPipeline.Serializable.ContentType);
                        if (!headers.ContainsKey(MessageHeaders.QualifiedTypeName))
                            headers.Add(MessageHeaders.QualifiedTypeName, typeof(T).FullName);
                        if (!headers.ContainsKey(MessageHeaders.CreationTime))
                            headers.Add(MessageHeaders.CreationTime, DateTimeOffset.UtcNow);
                        TransportMessage transportMessage = new TransportMessage(stream, headers);
                        return new TransportPipelineContext(runtimeContext, transportMessage);
                    })
                .Select(
                    context =>
                    {
                        ITransportPipelineContext transportPipelineContext = context;
                        foreach (var interceptor in interceptors)
                        {
                            transportPipelineContext = interceptor.Invoke(transportPipelineContext);
                        }
                        return transportPipelineContext;
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
