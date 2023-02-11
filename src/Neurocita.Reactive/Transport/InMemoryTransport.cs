using System.Globalization;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Neurocita.Reactive.Transport
{
    public class InMemoryTransport : ITransport
    {
        private readonly ITransport _innerTransport;

        public IObservable<T> Observe<T>(string nodePath) where T : ITransportMessage
        {
            return _innerTransport.Observe<T>(nodePath);
        }

        public IDisposable Sink<T>(IObservable<T> observable, string nodePath) where T : ITransportMessage
        {
            return _innerTransport.Sink(observable, nodePath);
        }

        public void Dispose()
        {
            _innerTransport.Dispose();
        }
    }

    internal class InMemoryP2PTransport : ITransport
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<ITransportMessage>> _queues = new ConcurrentDictionary<string, ConcurrentQueue<ITransportMessage>>();
        private BooleanDisposable disposable = new BooleanDisposable();

        public IObservable<T> Observe<T>(string nodePath)
            where T : ITransportMessage
        {
            if (disposable.IsDisposed)
                return Observable.Empty<T>();

            ConcurrentQueue<ITransportMessage> queue = _queues.GetOrAdd(nodePath, new ConcurrentQueue<ITransportMessage>());
            T message = default(T);
            ITransportMessage messageReference = message;
            
            return Observable
                    .Generate<int,T>(500
                                    , state => !disposable.IsDisposed
                                    , state => state
                                    , state =>
                                    {
                                        while (!disposable.IsDisposed && !queue.TryDequeue(out messageReference))
                                            Task.Delay(state);
                                        return message;
                                    });
        }

        public IDisposable Sink<T>(IObservable<T> observable, string nodePath)
            where T : ITransportMessage
        {
            if (disposable.IsDisposed)
                return Disposable.Empty;

            ConcurrentQueue<ITransportMessage> queue = _queues.GetOrAdd(nodePath, new ConcurrentQueue<ITransportMessage>());
            return observable.Subscribe(message => queue.Enqueue(message));
        }

        public void Dispose()
        {
            disposable.Dispose();
            _queues.Clear();
        }
    }

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
        
        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}
