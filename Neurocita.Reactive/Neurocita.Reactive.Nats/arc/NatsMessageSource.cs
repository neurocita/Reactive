using NATS.Client;
using NATS.Client.Rx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Neurocita.Reactive.Nats
{
    internal class NatsMessageSource : ITransportMessageSource
    {
        private readonly Lazy<IConnection> connection;
        private readonly string node;
        private readonly IObservable<IMessage<Stream>> messages;
        private CompositeDisposable subscribers = new CompositeDisposable();

        public NatsMessageSource(Lazy<IConnection> connection, string node)
        {
            this.connection = connection;
            this.node = node;
            this.messages = Observable.Create<IMessage<Stream>>(observer =>
            {
                IObservable<IMessage<Stream>> observable =
                    connection
                    .Value
                    .Observe(node)
                    .Select(msg =>
                    {
                        IDictionary<string, object> headers = msg.HasHeaders ? new Dictionary<string, object>() : null;
                        if (headers != null)
                        {
                            foreach (string key in msg.Header.Keys)
                            {
                                headers.Add(key, msg.Header[key]);
                            }

                            // ToDo: Standard heaers? ...
                            /*
                            foreach(string key in msg.Header.Keys)
                            {
                                switch (key)
                                {
                                    case "???":
                                        headers.Add(MessageHeaders.MessageId, msg.Header[key]);
                                        break;
                                }
                            }
                            */
                        }
                        Stream stream = new MemoryStream(msg.Data);
                        return new TransportMessage(stream, headers);
                    });
                IDisposable disposable = observable.Subscribe(observer);
                subscribers.Add(disposable);
                return disposable;
            });
        }

        public string Node => node;
        public IObservable<IMessage<Stream>> Messages => messages;

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (subscribers.IsDisposed)
                return;

            if (disposing)
            {
                subscribers.Dispose();

                if (connection.IsValueCreated)
                {
                    connection.Value.Drain();
                    connection.Value.Close();
                    connection.Value.Dispose();
                }
            }
        }
    }
}
