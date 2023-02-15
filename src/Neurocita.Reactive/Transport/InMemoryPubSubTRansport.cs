using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Neurocita.Reactive.Transport
{
    internal class InMemoryPubSubTransport : ITransport
    {
        private readonly ConcurrentDictionary<string, ISubject<ITransportMessage>> _topics = new ConcurrentDictionary<string, ISubject<ITransportMessage>>();
        private CompositeDisposable disposable = new CompositeDisposable();

        public IObservable<T> Observe<T>(string nodePath)
            where T : ITransportMessage
        {
            if (disposable.IsDisposed)
                return Observable.Empty<T>();

            ISubject<ITransportMessage> topic = _topics.GetOrAdd(nodePath, new Subject<ITransportMessage>());
            return topic.Select(message => (T)message).AsObservable();
        }

        public IDisposable Sink<T>(IObservable<T> observable, string nodePath)
            where T : ITransportMessage
        {
            if (disposable.IsDisposed)
                return Disposable.Empty;

            ISubject<ITransportMessage> topic = _topics.GetOrAdd(nodePath, new Subject<ITransportMessage>());
            IDisposable innerDisposable = observable.Subscribe(message => topic.OnNext(message));
            disposable.Add(innerDisposable);
            return innerDisposable;
        }
        
        public void Dispose() => disposable.Dispose();
    }

}