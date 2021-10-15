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

        internal static IObservable<IMessage<T>> AsObjectMessage<T>(this IObservable<T> observable, IDictionary<string, object> headers = null)
        {
            return observable.Select(instance => new ObjectMessage<T>(instance, headers));
        }

        internal static IObservable<IMessage<T>> AsObjectMessage<T>(this IObservable<ITransportPipelineContext> observable, IDeserializer deserializer)
        {
            return observable.Select(context => new ObjectMessage<T>(deserializer.Deserialize<T>(context.Message.Body), context.Message.Headers));
        }

        internal static IObservable<ITransportPipelineContext> AsTransportPipelineContext<T>(this IObservable<IMessage<T>> observable, IRuntimeContext runtimeContext, ISerializer serializer)
        {
            return observable.Select(message => new TransportPipelineContext(runtimeContext, new TransportMessage(serializer.Serialize(message.Body), message.Headers)));
        }

        internal static IObservable<ITransportPipelineContext> AsTransportPipelineContext<T>(this IObservable<T> observable, IRuntimeContext runtimeContext, ISerializer serializer, IDictionary<string, object> headers = null)
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

        //internal static IDisposable ToTransport(this IObservable<ITransportPipelineContext> observable, ITransport transport)
        //{
        //    return observable.Subscribe(transport);
        //}
    }
}
