using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Neurocita.Reactive
{
    internal class TransportInboundPipeline<T> : IDisposableObservable<T>
    {
        private bool disposed = false;
        private readonly IRuntimeContext runtimeContext;
        private readonly IDisposableObservable<ITransportPipelineContext> transportObservable;
        private readonly IDeserializer deserializer;
        private readonly IEnumerable<Func<ITransportPipelineContext, ITransportPipelineContext>> interceptors;

        internal TransportInboundPipeline(ITransportPipeline transportPipeline)
        {
            if (transportPipeline == null)
                throw new ArgumentNullException(nameof(transportPipeline));

            runtimeContext = transportPipeline.RuntimeContext;
            transportObservable = transportPipeline.Transport?.CreateInbound();
            deserializer = transportPipeline.Serializable?.CreateDeserializer();
            interceptors = transportPipeline.InboundInterceptors ?? new List<Func<ITransportPipelineContext, ITransportPipelineContext>>();

            if (runtimeContext == null)
                throw new ArgumentNullException(nameof(runtimeContext));
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
                .Select(context =>
                        {
                            ITransportPipelineContext transportPipelineContext = context;
                            foreach (var interceptor in interceptors)
                            {
                                transportPipelineContext = interceptor.Invoke(transportPipelineContext);
                            }
                            return transportPipelineContext;
                        })
                .Select(context => deserializer.Deserialize<T>(context.Message.Body))
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
