using System.Reactive.Disposables;

namespace Neurocita.Reactive
{
    internal class Endpoint : IEndpoint
    {
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly ITransportMessageFactory transportMessageFactory;
        private readonly ISerializerFactory serializerFactory;
        private readonly string path;

        public Endpoint(ITransportMessageFactory transportMessageFactory, ISerializerFactory serializerFactory, string path)
        {
            Util.CheckNullArgument(transportMessageFactory);
            Util.CheckNullArgument(serializerFactory);
            Util.CheckNullArgument(path);

            this.transportMessageFactory = transportMessageFactory;
            this.serializerFactory = serializerFactory;
            this.path = path;
        }

        public string Path => path;

        public ISinkEndpoint AsSink()
        {
            ISinkEndpoint sinkEndpoint = new SinkEndpoint(transportMessageFactory.CreateSink(path), serializerFactory.CreateSerializer());
            if (disposables.IsDisposed)
                sinkEndpoint.Dispose();
            else
            {
                // Pack the sink endpoint in a composite disposable which auto removes from registry on dispose
                CompositeDisposable disposable = new CompositeDisposable(sinkEndpoint);
                disposable.Add(Disposable.Create(() => disposables.Remove(disposable)));
                disposables.Add(disposable);
            }
            return sinkEndpoint;
        }

        public ISourceEndpoint AsSource()
        {
            ISourceEndpoint sourceEndpoint = new SourceEndpoint(transportMessageFactory.CreateSource(path), serializerFactory.CreateSerializer());
            if (disposables.IsDisposed)
                sourceEndpoint.Dispose();
            else
            {
                // Pack the source endpoint in a composite disposable which auto removes from registry on dispose
                CompositeDisposable disposable = new CompositeDisposable(sourceEndpoint);
                disposable.Add(Disposable.Create(() => disposables.Remove(disposable)));
                disposables.Add(disposable);
            }
            return sourceEndpoint;
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposables.IsDisposed)
                return;

            if (disposing)
                disposables.Dispose();
        }
    }
}
