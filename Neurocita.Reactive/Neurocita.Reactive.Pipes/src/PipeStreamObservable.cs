using System.Runtime.Serialization;

namespace Neurocita.Reactive.Pipes
{
    public static class PipeStreamObservable
    {
        public static IDisposableObservable<T> FromPipeStream<T>(string pipeName, IFormatter formatter = null)
        {
            return new PipeStreamServerConsumer<T>(pipeName, formatter);
        }

        public static IDisposableObservable<T> FromPipeStream<T>(string serverName, string pipeName, IFormatter formatter = null)
        {
            return new PipeStreamClientConsumer<T>(serverName, pipeName, formatter);
        }
    }
}