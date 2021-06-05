using System;
using System.Runtime.Serialization;

namespace Neurocita.Reactive.Pipes
{
    public static class ObservableExtensions
    {
        public static IDisposable ToPipeStream<T>(this IObservable<T> observable, string pipeName, IFormatter formatter = null)
        {
            return new PipeStreamServerProducer<T>(observable, pipeName, formatter);
        }

        public static IDisposable ToPipeStream<T>(this IObservable<T> observable, string serverName, string pipeName, IFormatter formatter = null)
        {
            return new PipeStreamClientProducer<T>(observable, serverName, pipeName, formatter);
        }
    }
}