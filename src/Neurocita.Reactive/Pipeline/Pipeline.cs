using System;
using Neurocita.Reactive.Serialization;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive.Pipeline
{
    internal class Pipeline : IPipeline
    {
        public ITransport Transport { get; }
        public ISerializer Serializer { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDisposable Execute<T>(string nodePath, IObservable<T> source)
        {
            throw new NotImplementedException();
        }

        public IObservable<T> Execute<T>(string nodePath)
        {
            throw new NotImplementedException();
        }
    }
}
