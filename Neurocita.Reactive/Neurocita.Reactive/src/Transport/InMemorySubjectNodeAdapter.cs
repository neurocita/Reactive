using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Neurocita.Reactive.Transport
{
    public class InMemorySubjectNodeAdapter : IInMemoryTransportNode
    {
        private readonly string path;
        private readonly Subject<IMessage<Stream>> subject = new Subject<IMessage<Stream>>();
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        internal InMemorySubjectNodeAdapter(string path)
        {
            this.path = path;
            disposables.Add(subject);
        }

        public string Path => path;

        public IObservable<IMessage<Stream>> Consume()
        {
            return subject.AsObservable();
        }

        public IDisposable Produce(IObservable<IMessage<Stream>> source)
        {
            IDisposable disposable = source.Subscribe(subject);
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
