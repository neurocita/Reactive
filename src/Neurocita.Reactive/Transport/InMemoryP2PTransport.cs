using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Neurocita.Reactive.Transport
{
    internal class InMemoryP2PTransport : ITransport
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<ITransportMessage>> _queues = new ConcurrentDictionary<string, ConcurrentQueue<ITransportMessage>>();
        private BooleanDisposable disposable = new BooleanDisposable();

        public IObservable<ITransportMessage> Observe(string nodePath)
        {
            if (disposable.IsDisposed)
                return Observable.Empty<ITransportMessage>();

            ConcurrentQueue<ITransportMessage> queue = _queues.GetOrAdd(nodePath, new ConcurrentQueue<ITransportMessage>());
            ITransportMessage message = default(ITransportMessage);
            ITransportMessage messageReference = message;
            
            return Observable
                    .Generate<int,ITransportMessage>(500
                                    , state => !disposable.IsDisposed
                                    , state => state
                                    , state =>
                                    {
                                        while (!disposable.IsDisposed && !queue.TryDequeue(out messageReference))
                                            Task.Delay(state);
                                        return message;
                                    });
        }

        public IDisposable Sink(IObservable<ITransportMessage> observable, string nodePath)
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
}