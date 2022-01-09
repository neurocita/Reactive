using System.Reactive.Disposables;
using Neurocita.Reactive.Transport;
using Neurocita.Reactive.Serialization;

namespace Neurocita.Reactive
{
    internal class Endpoint : IEndpoint
    {
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly ITransport transport;
        private readonly ISerializer serializer;
        private readonly string nodePath;

        public Endpoint(ITransport transport, ISerializer serializer, string nodePath)
        {
            Util.CheckNullArgument(transport);
            Util.CheckNullArgument(serializer);
            Util.CheckNullArgument(nodePath);

            this.transport = transport;
            this.serializer = serializer;
            this.nodePath = nodePath;
        }

        public string NodePath => nodePath;

        public ISinkEndpoint AsSink()
        {
            ISinkEndpoint sinkEndpoint = new SinkEndpoint(transport.CreateSink(nodePath), serializer);
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
            ISourceEndpoint sourceEndpoint = new SourceEndpoint(transport.CreateSource(nodePath), serializer);
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
