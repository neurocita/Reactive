using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Neurocita.Reactive.Transport
{
    public class InMemoryQueueNodeAdapter : IInMemoryTransportNode
    {
        private readonly string path;
        private readonly ConcurrentQueue<IMessage<Stream>> queue = new ConcurrentQueue<IMessage<Stream>>();
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        internal InMemoryQueueNodeAdapter(string path)
        {
            this.path = path;
            disposables.Add(new CancellationDisposable(cancellationTokenSource));
        }

        public string Path => path;

        public IObservable<IMessage<Stream>> Consume()
        {
            return Observable.Create<IMessage<Stream>>(observer =>
            {
                try
                {
                    while (!cancellationTokenSource.IsCancellationRequested)
                    {
                        if (queue.TryDequeue(out var message))
                            observer.OnNext(message);
                        else
                            Task.Delay(100);
                    }
                    observer.OnCompleted();
                }
                catch (Exception exception)
                {
                    observer.OnError(exception);
                }
                return Disposable.Empty;
            }); 
        }

        public IDisposable Produce(IObservable<IMessage<Stream>> source)
        {
            IDisposable disposable = source.Subscribe(message => queue.Enqueue(message));
            disposables.Add(disposable);
            return disposable;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposables.IsDisposed)
            {
                if (disposing)
                {
                    disposables.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
