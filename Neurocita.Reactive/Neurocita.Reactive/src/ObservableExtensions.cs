using System;
using System.IO;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Linq;
using System.Reactive.Linq;

namespace Neurocita.Reactive
{
    public static class ObservableExtensions
    {
        public static IObservable<IPipelineObjectContext<TDataContract>> Deserialize<TDataContract>(this IObservable<IPipelineTransportContext> observable, ISerializer serializer)
            where TDataContract : IDataContract
        {
            return observable.Select(context =>
            {
                TDataContract instancce = serializer.Deserialize<TDataContract>(context.Message.Body);
                return new PipelineObjectContext<TDataContract>(PipelineDirection.Inbound, new ObjectMessage<TDataContract>(instancce, context.Message.Headers));
            });
        }

        public static IObservable<IPipelineTransportContext> Serialize<TDataContract>(this IObservable<IPipelineObjectContext<TDataContract>> observable, ISerializer serializer)
            where TDataContract : IDataContract
        {
            return observable.Select(context =>
            {
                IDictionary<string, object> headers = context.Message.Headers ?? new Dictionary<string, object>();
                if (!headers.ContainsKey(MessageHeaders.ContentType) && !string.IsNullOrWhiteSpace(serializer.ContentType))
                    headers.Add(MessageHeaders.ContentType, serializer.ContentType);

                Stream stream = serializer.Serialize(context.Message.Body);
                TransportMessage transportMessage = new TransportMessage(stream, context.Message.Headers);
                return new PipelineTransportContext(context.Direction, transportMessage) as IPipelineTransportContext;
            });
        }

        public static IObservable<TDataContract> ToDataContract<TDataContract>(this IObservable<IPipelineObjectContext<TDataContract>> observable)
            where TDataContract : IDataContract
        {
            return observable.Select(context => context.Message.Body);
        }

        public static IObservable<IPipelineTransportContext> ToPipelineContext(this IObservable<IMessage<Stream>> observable)
      
        {
            return observable.Select(message =>
            {
                IDictionary<string, object> headers = message.Headers ?? new Dictionary<string, object>();
                if (!headers.ContainsKey(MessageHeaders.CreationTime))
                    headers.Add(MessageHeaders.CreationTime, DateTimeOffset.UtcNow);

                return new PipelineTransportContext(PipelineDirection.Inbound, message) as IPipelineTransportContext;
            });
        }

        public static IObservable<IPipelineObjectContext<TDataContract>> ToPipelineContext<TDataContract>(this IObservable<TDataContract> observable, IDictionary<string, object> messageHeaders = null)
            where TDataContract : IDataContract
        {
            return  observable.Select(instance =>
            {
                IDictionary<string, object> headers = messageHeaders ?? new Dictionary<string, object>();
                if (!headers.ContainsKey(MessageHeaders.QualifiedTypeName))
                    headers.Add(MessageHeaders.QualifiedTypeName, typeof(TDataContract).FullName);
                if (!headers.ContainsKey(MessageHeaders.CreationTime))
                    headers.Add(MessageHeaders.CreationTime, DateTimeOffset.UtcNow);

                var message = new ObjectMessage<TDataContract>(instance, headers);
                return new PipelineObjectContext<TDataContract>(PipelineDirection.Outbound, message) as IPipelineObjectContext<TDataContract>;
            });
        }

        public static IObservable<TValue> ToValue<TValue>(this IObservable<IValueTypeDataContract<TValue>> observable)
            where TValue : struct
        {
            return observable.Select(contract => contract.Value);
        }

        public static IObservable<IValueTypeDataContract<TValue>> ToDataContract<TValue>(this IObservable<TValue> observable)
            where TValue : struct
        {
            return observable.Select(value => new ValueTypeDataContract<TValue>(value));
        }

        public static IObservable<TPipelineContext> Task<TPipelineContext>(this IObservable<TPipelineContext> observable, Action<TPipelineContext> task)
            where TPipelineContext : IPipelineContext
        {
            observable.Do(context => task.Invoke(context));
            return observable;
        }

        public static IObservable<TPipelineContext> Task<TPipelineContext>(this IObservable<TPipelineContext> observable, IPipelineTask<TPipelineContext> task)
            where TPipelineContext : IPipelineContext
        {
            observable.Do(context => task.Run(context));
            return observable;
        }

        public static IObservable<TPipelineContext> Tasks<TPipelineContext>(this IObservable<TPipelineContext> observable, IEnumerable<Action<TPipelineContext>> tasks)
            where TPipelineContext : IPipelineContext
        {
            observable.Do(context =>
            {
                foreach (var task in tasks)
                {
                    task.Invoke(context);
                }
            });
            return observable;
        }

        public static IObservable<TPipelineContext> Tasks<TPipelineContext>(this IObservable<TPipelineContext> observable, IEnumerable<IPipelineTask<TPipelineContext>> tasks)
            where TPipelineContext : IPipelineContext
        {
            observable.Do(context =>
            {
                foreach (var task in tasks)
                {
                    task.Run(context);
                }
            });
            return observable;
        }

        public static IObservable<TPipelineContext> Tasks<TPipelineContext>(this IObservable<TPipelineContext> observable, IObservable<Action<TPipelineContext>> tasks)
            where TPipelineContext : IPipelineContext
        {
            return observable.Do(context =>
                                    tasks.Do(task =>
                                                task.Invoke(context)
                                            )
                                );
        }

        public static IObservable<TPipelineContext> Tasks<TPipelineContext>(this IObservable<TPipelineContext> observable, IObservable<IPipelineTask<TPipelineContext>> tasks)
            where TPipelineContext : IPipelineContext
        {
            return observable.Do(context =>
                                    tasks.Do(task =>
                                                task.Run(context)
                                            )
                                );
        }
        
        /*
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
        */
        //internal static IDisposable ToTransport(this IObservable<ITransportPipelineContext> observable, ITransport transport)
        //{
        //    return observable.Subscribe(transport);
        //}
    }
}
