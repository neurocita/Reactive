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

        private InMemoryTransport(ITransport innerTransport)
        {
            _innerTransport = innerTransport;
        }

        public static InMemoryTransport P2P() => new InMemoryTransport(new InMemoryP2PTransport());
        public static InMemoryTransport PubSub() => new InMemoryTransport(new InMemoryPubSubTransport());

        public IObservable<T> Observe<T>(string nodePath) where T : ITransportMessage => _innerTransport.Observe<T>(nodePath);
        public IDisposable Sink<T>(IObservable<T> observable, string nodePath) where T : ITransportMessage => _innerTransport.Sink(observable, nodePath);
        public void Dispose() => _innerTransport.Dispose();
    }
}
