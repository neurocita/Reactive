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
        private readonly string path;
        private readonly IObservable<IMessage<Stream>> messages;
        private readonly CompositeDisposable subscribers = new CompositeDisposable();
        private readonly IDisposable refCounter;
        private readonly Lazy<INATSObservable<Msg>> natsObservanle;
        
        public NatsMessageSource(NatsConnectionManager sharedConnection, string path)
        {
            this.path = path;
            refCounter = sharedConnection.GetDisposable();
            natsObservanle = sharedConnection.GetMessages(path);
            messages = Observable.Create<IMessage<Stream>>(observer =>
            {
                IObservable<IMessage<Stream>> observable =
                    natsObservanle
                    .Value
                    .Select(msg =>
                    {
                        IDictionary<string, object> headers = msg.HasHeaders ? new Dictionary<string, object>() : null;
                        if (headers != null)
                        {
                            foreach (string key in msg.Header.Keys)
                            {
                                headers.Add(key, msg.Header[key]);
                                Console.WriteLine("Header: ", key);
                            }

                            if (!string.IsNullOrWhiteSpace(msg.Reply))
                                if (headers.ContainsKey(MessageHeaders.ReplyTo))
                                    headers[MessageHeaders.ReplyTo] = msg.Reply;
                                else
                                    headers.Add(MessageHeaders.ReplyTo, msg.Reply);
                            // ... ??? ...

                            // ToDo: Standard headers? ...
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

        public string Path => path;
        public IObservable<IMessage<Stream>> Messages => messages;

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (subscribers.IsDisposed)
                return;

            if (disposing)
            {
                subscribers.Dispose();
                refCounter.Dispose();
            }
        }
    }
}
