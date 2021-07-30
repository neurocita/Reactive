using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Neurocita.Reactive
{
    public static class ObservableExtensions
    {
        public static IEndpoint<T> ToEndpoint<T>(this IObservable<T> observable, IEndpointConfiguration configuration)
        {
            return configuration.Create<T>(observable);
        }

        internal static IObservable<ITransportPipelineContext> AsPipeline<T>(this IObservable<T> observable, IRuntimeContext runtimeContext, ISerializer serializer, IDictionary<object, object> headers = null)
        {
            return observable.Select(instance => new TransportPipelineContext(runtimeContext, new TransportMessage(serializer.Serialize(instance), headers)));
        }

        internal static IObservable<ITransportPipelineContext> Intercept(this IObservable<ITransportPipelineContext> observable, Func<ITransportPipelineContext, ITransportPipelineContext> interceptor)
        {
            return observable.Do(context => interceptor(context));
        }

        internal static IObservable<ITransportPipelineContext> Intercept(this IObservable<ITransportPipelineContext> observable, ITransportPipelineInterceptor interceptor)
        {
            return observable.Do(context => interceptor.Invoke(context));
        }

        internal static IDisposable ToTransport(this IObservable<ITransportPipelineContext> observable, ITransport transport)
        {
            return observable.Subscribe(transport);
        }
    }
}
