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

        public ITransport Transport { get; }
        public ISerializer Serializer { get; }

        public void Dispose()
        {
            disposables.Dispose();
            Transport.Dispose();
        }

        public IDisposable Execute<T>(string nodePath, IObservable<T> source)
        {
            IDisposable disposable = source
                                        .ToMessage<T>((IDictionary<string,object>) null)
                                        .Serialize<T,ISerializer>(Serializer)
                                        .To(Transport, nodePath);
            disposables.Add(disposable);
            return disposable;
        }

        public IObservable<T> Execute<T>(string nodePath)
        {
            return Transport
                    .Observe<ITransportMessage>(nodePath)
                    .Deserialize<T,ITransportMessage,ISerializer>(Serializer)
                    .FromMessage<T,IMessage<T>>();
        }
    }
}
