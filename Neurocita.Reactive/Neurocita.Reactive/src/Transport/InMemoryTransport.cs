using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Neurocita.Reactive.Transport
{
    public class InMemoryTransport : ITransport
    {
        private readonly ConcurrentDictionary<string, IInMemoryTransportNode> nodes;
        private bool disposedValue;

        public InMemoryTransport()
        {
            nodes = new ConcurrentDictionary<string, IInMemoryTransportNode>();
        }

        public InMemoryTransport(KeyedCollection<string, IInMemoryTransportNode> nodes)
        {
            this.nodes = new ConcurrentDictionary<string, IInMemoryTransportNode>(nodes.ToDictionary(node => node.Path));
        }

        public bool AddQueue(string path)
        {
            return nodes.TryAdd(path, new InMemoryQueueNodeAdapter(path));
        }

        public bool AddTopic(string path)
        {
            return nodes.TryAdd(path, new InMemorySubjectNodeAdapter(path));
        }

        public bool AddNode(IInMemoryTransportNode node)
        {
            return nodes.TryAdd(node.Path, node);
        }

        public bool RemoveNode(IInMemoryTransportNode node)
        {
            if (nodes.TryGetValue(node.Path, out IInMemoryTransportNode newNode))
                newNode.Dispose();
            return nodes.TryRemove(node.Path, out _);
        }

        public bool RemoveNode(string path)
        {
            if (nodes.TryGetValue(path, out IInMemoryTransportNode newNode))
                newNode.Dispose();
            return nodes.TryRemove(path, out _);
        }

        public IObservable<IMessage<Stream>> Observe(string path)
        {
            return nodes[path].Consume();
        }

        public IDisposable Sink(string path, IObservable<IMessage<Stream>> observable)
        {
            return nodes[path].Produce(observable);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var node in nodes.Values)
                    {
                        node.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
