using NATS.Client;
using NATS.Client.Rx;
using System;
using System.Reactive.Disposables;

namespace Neurocita.Reactive.Transport
{
    internal class NatsTransport : ITransport
    {
        private readonly Options options;
        private readonly object mutex = new object();
        private Lazy<IConnection> connection;
        private RefCountDisposable refCountDisposable;

        internal NatsTransport(Options options)
        {
            this.options = options;
            connection = new Lazy<IConnection>(OnConnectionInit);
            refCountDisposable = new RefCountDisposable(Disposable.Create(OnDisposed));
        }

        public ITransportMessageSource CreateSource(string source)
        {
            return new NatsTransportMessageSource(this, source);
        }

        public ITransportMessageSink CreateSink(string destination)
        {
            return new NatsTransportMessageSink(this, destination);
        }

        internal Lazy<INATSObservable<Msg>> GetMessages(string subject)
        {
            return new Lazy<INATSObservable<Msg>>(() => connection.Value.Observe(subject));
        }

        internal void Publish(Msg msg)
        {
            lock (mutex)
            {
                connection.Value.Publish(msg);
            }
        }

        internal IDisposable GetDisposable()
        {
            IDisposable disposable = refCountDisposable.GetDisposable();
            refCountDisposable.Dispose();
            return disposable;
        }

        private Func<IConnection> OnConnectionInit
        {
            get
            {
                return () =>
                {
                    lock (mutex)
                    {
                        return new ConnectionFactory().CreateConnection(options);
                    }
                };
            }
        }

        private Action OnDisposed
        {
            get
            {
                return () =>
                {
                    lock (mutex)
                    {
                        connection.Value.Dispose();
                        connection = new Lazy<IConnection>(OnConnectionInit);
                        refCountDisposable = new RefCountDisposable(Disposable.Create(OnDisposed));
                    }
                };
            }
        }
    }
}
