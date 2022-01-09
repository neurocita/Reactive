using System;
using System.IO;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Linq;
using System.Reactive.Linq;
using Neurocita.Reactive.Transport;
using Neurocita.Reactive.Serialization;

namespace Neurocita.Reactive.Pipeline
{
    public static class ObservableExtensions
    {
        public static IObservable<IPipelineObjectContext<TDataContract>> Deserialize<TDataContract>(this IObservable<IPipelineTransportContext> observable, ISerializer serializer)
            where TDataContract : IDataContract
        {
            return observable
                .TakeUntil(other => (string)other.Message?.Headers?[MessageHeaders.ObservableEvent] == "OnCompleted")
                .SkipLast(1)
                //.Concat(Observable.Empty<IPipelineTransportContext>())
                .Select(context =>
                {
                    IDictionary<string, object> headers = context.Message?.Headers ?? new Dictionary<string, object>();
                    if (!headers.ContainsKey(MessageHeaders.ObservableEvent))
                        headers.Add(MessageHeaders.ObservableEvent, "OnNext");

                    switch (headers[MessageHeaders.ObservableEvent])
                    {
                        case "OnError":
                            Exception exception = serializer.Deserialize<Exception>(context.Message.Body);
                            throw exception;

                        default:
                            TDataContract instance = serializer.Deserialize<TDataContract>(context.Message.Body);
                            return new PipelineObjectContext<TDataContract>(PipelineDirection.Inbound, new ObjectMessage<TDataContract>(instance, context.Message.Headers));
                    }
                });
        }

        public static IObservable<IPipelineObjectContext<TDataContract>> Deserialize<TDataContract>(this IObservable<IPipelineTransportContext> observable, ISerializerFactory serializerFactory)
            where TDataContract : IDataContract
        {
            return observable.Deserialize<TDataContract>(serializerFactory.Create());
        }

        public static IObservable<IPipelineTransportContext> Serialize<TDataContract>(this IObservable<IPipelineObjectContext<TDataContract>> observable, ISerializer serializer)
            where TDataContract : IDataContract
        {
            var onCompleted = new Lazy<IPipelineTransportContext>(() =>
            {
                IDictionary<string, object> headers = new Dictionary<string, object>();
                headers.Add(MessageHeaders.ContentType, serializer.ContentType);
                headers.Add(MessageHeaders.ObservableEvent, "OnCompleted");

                TransportMessage transportMessage = new TransportMessage(null, headers);
                return new PipelineTransportContext(PipelineDirection.Outbound, transportMessage) as IPipelineTransportContext;
            });

            return observable
            .Select(context =>
            {
                IDictionary<string, object> headers = context.Message.Headers ?? new Dictionary<string, object>();
                if (!headers.ContainsKey(MessageHeaders.ContentType) && !string.IsNullOrWhiteSpace(serializer.ContentType))
                    headers.Add(MessageHeaders.ContentType, serializer.ContentType);
                headers[MessageHeaders.ObservableEvent] = "OnNext";

                Stream stream = serializer.Serialize(context.Message.Body);
                TransportMessage transportMessage = new TransportMessage(stream, headers);
                return new PipelineTransportContext(context.Direction, transportMessage) as IPipelineTransportContext;
            })
            .Concat(Observable.Return<IPipelineTransportContext>(onCompleted.Value))
            .Catch<IPipelineTransportContext, Exception>(exception =>
            {
                IDictionary<string, object> headers = new Dictionary<string, object>();
                if (!headers.ContainsKey(MessageHeaders.ContentType) && !string.IsNullOrWhiteSpace(serializer.ContentType))
                    headers.Add(MessageHeaders.ContentType, serializer.ContentType);
                headers[MessageHeaders.ObservableEvent] = "OnError";
                headers[MessageHeaders.QualifiedTypeName] = exception.GetType().FullName;
                
                Stream stream = serializer.Serialize(exception);
                TransportMessage transportMessage = new TransportMessage(stream, headers);
                IPipelineTransportContext pipelineTransportContext = new PipelineTransportContext(PipelineDirection.Outbound, transportMessage);
                return Observable.Return<IPipelineTransportContext>(pipelineTransportContext);
            });
        }

        public static IObservable<IPipelineTransportContext> Serialize<TDataContract>(this IObservable<IPipelineObjectContext<TDataContract>> observable, ISerializerFactory serializerFactory)
            where TDataContract : IDataContract
        {
            return observable.Serialize<TDataContract>(serializerFactory.Create());
        }

        public static IObservable<TDataContract> ToDataContract<TDataContract>(this IObservable<IPipelineObjectContext<TDataContract>> observable)
            where TDataContract : IDataContract
        {
            return observable.Select(context => context.Message.Body);
        }

        public static IObservable<IMessage<Stream>> ToTransportMessage(this IObservable<IPipelineTransportContext> observable)
        {
            return observable.Select(context =>
            {
                return context.Message;
            });
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
    }
}
