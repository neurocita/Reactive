//using Common.Logging;
using NATS.Client;
using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Neurocita.Reactive.Nats
{
    internal class NatsMessageSink : ITransportMessageSink
    {
        private readonly string node;
        private readonly CompositeDisposable subscribers = new CompositeDisposable();
        //private readonly Common.Logging.ILog log = LogManager.GetLogger<NatsMessageSink>();
        private readonly IDisposable refCounter;
        private Action<Msg> publish;
        
        public NatsMessageSink(NatsSharedConnection sharedConnection, string node)
        {
            this.node = node;
            refCounter = sharedConnection.GetDisposable();
            publish = sharedConnection.Publish;
        }

        public string Node => node;

        public IDisposable Observe(IObservable<IMessage<Stream>> messages)
        {
            CancellationDisposable subscriber = new CancellationDisposable();
            IObserver<IMessage<Stream>> observer = Observer.Create<IMessage<Stream>>(
                message =>
                {
                    if (message?.Body?.Length <= 0)
                        return;

                    message.Body.Position = 0;
                    byte[] data = new byte[message.Body.Length];
                    if (message.Body.Read(data, 0, data.Length) != message.Body.Length)
                        throw new ArgumentOutOfRangeException(nameof(message));

                    Msg msg = new Msg(node, data);

                    if (message.Headers.ContainsKey(MessageHeaders.ReplyTo))
                        msg.Reply = message.Headers[MessageHeaders.ReplyTo] as string;
                    // ToDo: Headers, reply, ...

                    publish.Invoke(msg);
                },
                exception =>
                {
                    //log.Error(exception.Message, exception);
                },
                () =>
                {
                    //log.Info("Message stream completed.");
                });
            // ToDo: Error and completion handling ...
            messages
                .Finally(() =>
                {
                    subscriber.Dispose();
                    subscribers.Remove(subscriber);
                })
                .Subscribe(observer, subscriber.Token);
            subscribers.Add(subscriber);
            return subscriber;
        }

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
