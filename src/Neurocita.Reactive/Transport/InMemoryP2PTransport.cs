using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Neurocita.Reactive.Utilities;

namespace Neurocita.Reactive.Transport
{
    internal class InMemoryP2PTransport : ITransport
    {
        private readonly ConcurrentDictionary<string, PointToPointSubject<ITransportMessage>> _queues = new ConcurrentDictionary<string, PointToPointSubject<ITransportMessage>>();
        private CompositeDisposable disposables = new CompositeDisposable();

        public IObservable<ITransportMessage> Observe(string nodePath)
        {
            if (disposables.IsDisposed)
                return Observable.Empty<ITransportMessage>();

            return _queues
                    .GetOrAdd(nodePath, new PointToPointSubject<ITransportMessage>())
                    .AsObservable();
        }

        public IDisposable Sink(IObservable<ITransportMessage> observable, string nodePath)
        {
            if (disposables.IsDisposed)
                return Disposable.Empty;
            
            PointToPointSubject<ITransportMessage> queue = _queues.GetOrAdd(nodePath, new PointToPointSubject<ITransportMessage>());
            IDisposable innerDisposable = observable.Subscribe(message => queue.OnNext(message));
            disposables.Add(innerDisposable);
            return innerDisposable;
        }

        public void Dispose() => disposables.Dispose();
    }
}