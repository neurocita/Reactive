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
        private CompositeDisposable disposables = new CompositeDisposable();

        public IObservable<ITransportMessage> Observe(string nodePath)
         {
            if (disposables.IsDisposed)
                return Observable.Empty<ITransportMessage>();

            return _topics
                    .GetOrAdd(nodePath, new Subject<ITransportMessage>())
                    .AsObservable();
        }

        public IDisposable Sink(IObservable<ITransportMessage> observable, string nodePath)
        {
            if (disposables.IsDisposed)
                return Disposable.Empty;

            
            ISubject<ITransportMessage> topic = _topics.GetOrAdd(nodePath, new Subject<ITransportMessage>());
            IDisposable innerDisposable = observable.Subscribe(message => topic.OnNext(message));
            disposables.Add(innerDisposable);
            return innerDisposable;
        }
        
        public void Dispose() => disposables.Dispose();
    }

}