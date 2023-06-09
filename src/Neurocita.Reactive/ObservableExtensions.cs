using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Reactive.Linq;
using Neurocita.Reactive.Pipeline;
using Neurocita.Reactive.Serialization;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive
{
    public delegate ITransportMessage TransportMessageFactory<TMessage>(Stream body, IDictionary<string,object> headers)
        where TMessage : ITransportMessage;

    public static class ObservableExtensions
    {
        public static IDisposable SendTo(this IObservable<ITransportMessage> transportMessages, ITransport transport, string nodePath)
            => transport.Sink(transportMessages, nodePath);

        public static IDisposable SendTo(this IObservable<ITransportMessage> transportMessages, Func<ITransport> transportFactory, string nodePath)
            => transportFactory.Invoke().Sink(transportMessages, nodePath);

        public static IDisposable SendTo<TState>(this IObservable<ITransportMessage> transportMessages, Func<TState,ITransport> transportFactory, TState state, string nodePath)
            => transportFactory.Invoke(state).Sink(transportMessages, nodePath);

        public static IObservable<ITransportMessage> Pack<TInput>(this IObservable<IMessage<TInput>> observable, ISerializer serializer, Func<Stream,IDictionary<string,object>,ITransportMessage> transportMessageFactory)
        {
            return observable
                .Select(message =>
                {
                    ITransportMessage transformed = transportMessageFactory.Invoke(serializer.Serialize(message.Body), message.Headers);
                    if (!transformed.Headers.ContainsKey(MessageHeaders.ContentType))
                        transformed.Headers[MessageHeaders.ContentType] = serializer.ContentType;
                    if (!transformed.Headers.ContainsKey(MessageHeaders.ContentTypeFullName))
                        transformed.Headers[MessageHeaders.ContentTypeFullName] = typeof(TInput);
                    return transformed;
                })
                .Concat(
                    Observable.Return<ITransportMessage>(
                        transportMessageFactory.Invoke(null, new Dictionary<string,object>()
                        {
                            { MessageHeaders.ContentTypeFullName, typeof(TInput) },
                            { MessageHeaders.ContentType, serializer.ContentType }
                        })))
                .Catch<ITransportMessage,Exception>(exception =>
                    Observable.Return<ITransportMessage>(
                        transportMessageFactory.Invoke(serializer.Serialize(exception), new Dictionary<string, object>()
                        {
                            { MessageHeaders.ContentTypeFullName, exception.GetType() },
                            { MessageHeaders.ContentType, serializer.ContentType }
                        })));
        }

        public static IObservable<ITransportMessage> Pack<TInput>(this IObservable<IMessage<TInput>> observable, Func<ISerializer> serializerFactory,Func<Stream,IDictionary<string,object>,ITransportMessage> transportMessageFactory)
            => observable.Pack<TInput>(serializerFactory.Invoke(), transportMessageFactory);

        public static IObservable<ITransportMessage> Pack<TInput,TState>(this IObservable<IMessage<TInput>> observable, Func<TState,ISerializer> serializerFactory, TState state, Func<Stream,IDictionary<string,object>,ITransportMessage> transportMessageFactory)
            => observable.Pack<TInput>(serializerFactory.Invoke(state), transportMessageFactory);

        public static IObservable<ITransportMessage> Pack<TInput>(this IObservable<IMessage<TInput>> observable, ISerializer serializer)
            => observable.Pack<TInput>(serializer, (body,headers) => new TransportMessage(body, headers));

        public static IObservable<ITransportMessage> Pack<TInput>(this IObservable<IMessage<TInput>> observable, Func<ISerializer> serializerFactory)
            => observable.Pack<TInput>(serializerFactory.Invoke(), (body,headers) => new TransportMessage(body, headers));

        public static IObservable<ITransportMessage> Pack<TInput,TState>(this IObservable<IMessage<TInput>> observable, Func<TState,ISerializer> serializerFactory, TState state)
            => observable.Pack<TInput>(serializerFactory.Invoke(state), (body,headers) => new TransportMessage(body, headers));

        public static IObservable<IMessage<TOutput>> Unpack<TOutput>(this IObservable<ITransportMessage> observable, ISerializer serializer)
        {
            return observable
                .TakeWhile(message => message?.Body != null || message?.Body?.Length > 0)       // Empty body is treated as completed event
                .Select(message =>
                {
                    try
                    {
                        TOutput result = serializer.Deserialize<TOutput>(message.Body);
                        if (message.Headers.ContainsKey(MessageHeaders.ContentType))
                            message.Headers.Remove(MessageHeaders.ContentType);
                        return new Message<TOutput>(result, message.Headers);
                    }
                    catch
                    {
                        Exception exception = serializer.Deserialize<Exception>(message.Body);  // Try to deserialize the exception content and throw it
                        throw exception;
                    }
                });
        }

        public static IObservable<IMessage<TOutput>> Unpack<TOutput>(this IObservable<ITransportMessage> observable, Func<ISerializer> serializerFactory)
            => observable.Unpack<TOutput>(serializerFactory.Invoke());

        public static IObservable<IMessage<TOutput>> Unpack<TOutput,TState>(this IObservable<ITransportMessage> observable, Func<TState,ISerializer> serializerFactory, TState state)
            => observable.Unpack<TOutput>(serializerFactory.Invoke(state));

        public static IObservable<IMessage<TInput>> Wrap<TInput>(this IObservable<TInput> observable, IDictionary<string,object> headers)
            => observable.Select(t => new Message<TInput>(t, headers));

        public static IObservable<IMessage<TInput>> Wrap<TInput>(this IObservable<TInput> observable, Func<IDictionary<string,object>> headerFactory)
            => observable.Select(t => new Message<TInput>(t, headerFactory.Invoke()));

        public static IObservable<IMessage<TInput>> Wrap<TInput, TState>(this IObservable<TInput> observable, Func<TState,IDictionary<string,object>> headerFactory, TState state)
            => observable.Select(t => new Message<TInput>(t, headerFactory.Invoke(state)));

        public static IObservable<TOutput> Unwrap<TOutput>(this IObservable<IMessage<TOutput>> observable)
            => observable.Select(message => message.Body);

        public static IObservable<TMessage> Intercept<TMessage>(this IObservable<TMessage> observable, Func<TMessage,TMessage> interceptor)
        {
            return observable.Select<TMessage,TMessage>(interceptor);
        }  

        public static IObservable<TMessage> Intercept<TMessage,TState>(this IObservable<TMessage> observable, Func<TMessage,TState,TMessage> interceptor, TState state)
        {
            return observable.Select<TMessage,TMessage>(message => interceptor.Invoke(message, state));
        }  
    }
}