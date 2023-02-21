using System.Collections.Generic;
using System.IO;
using System;
using System.Reactive.Linq;
using Neurocita.Reactive.Pipeline;
using Neurocita.Reactive.Serialization;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive
{
    public delegate TMessage TransportMessageFactory<TMessage>(Stream body, IDictionary<string,object> headers)
        where TMessage : ITransportMessage;

    public static class ObservableExtensions
    {
        public static IDisposable To<TMessage,TTransport>(this IObservable<TMessage> transportMessages, TTransport transport, string nodePath)
            where TMessage : ITransportMessage
            where TTransport : ITransport
        {
            return transport.Sink(transportMessages, nodePath);
        }

        public static IDisposable To<TMessage,TTransport>(this IObservable<TMessage> transportMessages, Func<TTransport> transportFactory, string nodePath)
        where TMessage : ITransportMessage
        where TTransport : ITransport
        {
            return transportFactory.Invoke().Sink(transportMessages, nodePath);
        }

        public static IDisposable To<TState,TMessage,TTransport>(this IObservable<TMessage> transportMessages, Func<TState,TTransport> transportFactory, TState state, string nodePath)
            where TMessage : ITransportMessage
            where TTransport : ITransport
        {
            return transportFactory.Invoke(state).Sink(transportMessages, nodePath);
        }

        public static IObservable<TMessage> Serialize<TInput,TSerializer,TMessage>(this IObservable<IMessage<TInput>> observable, TSerializer serializer, TransportMessageFactory<TMessage> transportMessageFactory)
            where TSerializer : ISerializer
            where TMessage : ITransportMessage
        {
            return observable
                .Select(message =>
                {
                    TMessage transformed = transportMessageFactory.Invoke(serializer.Serialize(message.Body), message.Headers);
                    if (!transformed.Headers.ContainsKey(MessageHeaders.ContentType))
                        transformed.Headers[MessageHeaders.ContentType] = serializer.ContentType;
                    if (!transformed.Headers.ContainsKey(MessageHeaders.ContentTypeFullName))
                        transformed.Headers[MessageHeaders.ContentTypeFullName] = typeof(TInput);
                    return transformed;
                })
                .Concat(
                    Observable.Return<TMessage>(
                        transportMessageFactory.Invoke(null, new Dictionary<string,object>()
                        {
                            { MessageHeaders.ContentTypeFullName, typeof(TInput) },
                            { MessageHeaders.ContentType, serializer.ContentType }
                        })))
                .Catch<TMessage,Exception>(exception =>
                    Observable.Return<TMessage>(
                        transportMessageFactory.Invoke(serializer.Serialize(exception), new Dictionary<string, object>()
                        {
                            { MessageHeaders.ContentTypeFullName, exception.GetType() },
                            { MessageHeaders.ContentType, serializer.ContentType }
                        })));
        }

        public static IObservable<TMessage> Serialize<TInput,TSerializer,TMessage>(this IObservable<IMessage<TInput>> observable, Func<TSerializer> serializerFactory, TransportMessageFactory<TMessage> transportMessageFactory)
            where TSerializer : ISerializer
            where TMessage : ITransportMessage
        {
            return observable.Serialize<TInput,TSerializer,TMessage>(serializerFactory.Invoke(), transportMessageFactory);
        }

        public static IObservable<TMessage> Serialize<TInput,TSerializer,TMessage,TState>(this IObservable<IMessage<TInput>> observable, Func<TState,TSerializer> serializerFactory, TState state, TransportMessageFactory<TMessage> transportMessageFactory)
            where TSerializer : ISerializer
            where TMessage : ITransportMessage
        {
            return observable.Serialize<TInput,TSerializer,TMessage>(serializerFactory.Invoke(state), transportMessageFactory);
        }

        public static IObservable<ITransportMessage> Serialize<TInput,TSerializer>(this IObservable<IMessage<TInput>> observable, TSerializer serializer)
            where TSerializer : ISerializer
        {
            return observable.Serialize<TInput,TSerializer,TransportMessage>(serializer, (body,headers) => new TransportMessage(body, headers));
        }

        public static IObservable<ITransportMessage> Serialize<TInput,TSerializer>(this IObservable<IMessage<TInput>> observable, Func<TSerializer> serializerFactory)
            where TSerializer : ISerializer
        {
            return observable.Serialize<TInput,TSerializer,TransportMessage>(serializerFactory.Invoke(), (body,headers) => new TransportMessage(body, headers));
        }

        public static IObservable<ITransportMessage> Serialize<TInput,TSerializer,TState>(this IObservable<IMessage<TInput>> observable, Func<TState,TSerializer> serializerFactory, TState state)
            where TSerializer : ISerializer
        {
            return observable.Serialize<TInput,TSerializer,TransportMessage>(serializerFactory.Invoke(state), (body,headers) => new TransportMessage(body, headers));
        }

        public static IObservable<IMessage<T>> Deserialize<T,TMessage,TSerializer>(this IObservable<TMessage> observable, TSerializer serializer)
            where TMessage : ITransportMessage
            where TSerializer : ISerializer
        {
            return observable
                .TakeWhile(message => message?.Body != null || message?.Body?.Length > 0)       // Empty body is treated as completed event
                .Select(message =>
                {
                    try
                    {
                        T result = serializer.Deserialize<T>(message.Body);
                        if (message.Headers.ContainsKey(MessageHeaders.ContentType))
                            message.Headers.Remove(MessageHeaders.ContentType);
                        return new Message<T>(result, message.Headers);
                    }
                    catch
                    {
                        Exception exception = serializer.Deserialize<Exception>(message.Body);  // Try to deserialize the exception content and throw it
                        throw exception;
                    }
                });
        }

        public static IObservable<IMessage<T>> Deserialize<T,TMessage,TSerializer>(this IObservable<TMessage> observable, Func<TSerializer> serializerFactory)
            where TMessage : ITransportMessage
            where TSerializer : ISerializer
        {
            return observable.Deserialize<T,TMessage,TSerializer>(serializerFactory.Invoke());
        }

        public static IObservable<IMessage<T>> Deserialize<T,TMessage,TState,TSerializer>(this IObservable<TMessage> observable, Func<TState,TSerializer> serializerFactory, TState state)
            where TMessage : ITransportMessage
            where TSerializer : ISerializer
        {
            return observable.Deserialize<T,TMessage,TSerializer>(serializerFactory.Invoke(state));
        }

        public static IObservable<IMessage<T>> ToMessage<T>(this IObservable<T> observable, IDictionary<string,object> headers)
        {
            return observable.Select(t => new Message<T>(t, headers));
        }

        public static IObservable<IMessage<T>> ToMessage<T>(this IObservable<T> observable, Func<IDictionary<string,object>> headerFactory)
        {
            return observable.Select(t => new Message<T>(t, headerFactory.Invoke()));
        }

        public static IObservable<IMessage<T>> ToMessage<T, TState>(this IObservable<T> observable, Func<TState,IDictionary<string,object>> headerFactory, TState state)
        {
            return observable.Select(t => new Message<T>(t, headerFactory.Invoke(state)));
        }

        public static IObservable<T> FromMessage<T,TMessage>(this IObservable<TMessage> observable)
            where TMessage : IMessage<T>
        {
            return observable.Select(message => message.Body);
        }
    }
}
