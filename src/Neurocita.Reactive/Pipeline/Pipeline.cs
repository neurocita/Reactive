using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Neurocita.Reactive.Serialization;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive.Pipeline
{
    internal class Pipeline : IPipeline
    {
        private CompositeDisposable disposables = new CompositeDisposable();

        public Pipeline(ITransport transport, ISerializer serializer)
        {
            Transport = transport;
            Serializer = serializer;
        }

        public ITransport Transport { get; }
        public ISerializer Serializer { get; }

        public void Dispose()
        {
            disposables.Dispose();
            Transport.Dispose();            // Ensure transport disposal at latest
        }

        public IDisposable Execute<TValue,TState>(string nodePath, IObservable<TValue> source, Func<TState,IDictionary<string,object>> headerFactory, TState state)
        {
            IDisposable disposable = source
                                        .Wrap(headerFactory.Invoke(state))
                                        .Pack(Serializer)
                                        .SendTo(Transport, nodePath);
            disposables.Add(disposable);
            return disposable;
        }

        public IDisposable Execute<TValue>(string nodePath, IObservable<TValue> source, Func<IDictionary<string,object>> headerFactory)
            => Execute<TValue,object>(nodePath, source, (state) => headerFactory.Invoke(), null);

        public IDisposable Execute<TValue>(string nodePath, IObservable<TValue> source)
            => Execute<TValue,object>(nodePath, source, (state) => null, null);

        public IObservable<TValue> Execute<TValue>(string nodePath)
        {
            return Transport
                    .Observe(nodePath)
                    .Unpack<TValue>(Serializer)
                    .Unwrap();
        }
    }
}
