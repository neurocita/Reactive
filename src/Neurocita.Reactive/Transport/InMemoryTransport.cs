using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Neurocita.Reactive.Utilities;

namespace Neurocita.Reactive.Transport
{
    public class InMemoryTransport : ITransport
    {
        private readonly ConcurrentDictionary<string, ISubject<ITransportMessage>> _topics = new ConcurrentDictionary<string, ISubject<ITransportMessage>>();
        private CompositeDisposable disposables = new CompositeDisposable();

        public InMemoryTransport(InMemoryExchangePattern exchangePattern)
        {
            ExchangePattern = exchangePattern;
        }

        public static InMemoryTransport P2P() => new InMemoryTransport(InMemoryExchangePattern.PointToPoint);
        public static InMemoryTransport PubSub() => new InMemoryTransport(InMemoryExchangePattern.PublishSubscribe);

        public InMemoryExchangePattern ExchangePattern { get;}

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
            
            ISubject<ITransportMessage> topic = _topics.GetOrAdd(nodePath, InMemoryTransport.GetSubjectFactory<ITransportMessage>(ExchangePattern).Invoke());
            IDisposable innerDisposable = observable.Subscribe(message => topic.OnNext(message));
            disposables.Add(innerDisposable);
            return innerDisposable;
        }

        public void Dispose() => disposables.Dispose();

        private static Func<ISubject<T>> GetSubjectFactory<T>(InMemoryExchangePattern exchangePattern)
        {
            switch (exchangePattern)
            {
                case InMemoryExchangePattern.PublishSubscribe:
                    return () => new Subject<T>();

                case InMemoryExchangePattern.PointToPoint:
                    return () => new PointToPointSubject<T>();

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchangePattern));
            }
        }
    }
}
