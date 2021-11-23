using Amqp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Subjects;
using Amqp.Framing;
using System.IO;
using Amqp.Listener;

namespace Neurocita.Reactive.Amqp
{
#if _NEVER
    internal class AmqpTransport : ITransport
    {
        private readonly IRuntimeContext runtimeContext;
        private readonly Address address;
        private readonly string node;
        private bool disposed = false;

        private Connection connection;
        private readonly ContainerHost containerhost;
        private readonly IList<Tuple<ReceiverLink,IObserver<IPipelineTransportContext>>> receivers = new List<Tuple<ReceiverLink, IObserver<IPipelineTransportContext>>>();
        SenderLink senderLink;
        IDisposable senderDisposable;

        public AmqpTransport(IRuntimeContext runtimeContext, string uri, string node, IObservable<IPipelineTransportContext> observable)
            : this(runtimeContext, uri, node)
        {
            senderDisposable = observable.Subscribe(this);
        }

        public AmqpTransport(IRuntimeContext runtimeContext, string uri, string node)
        {
            if (runtimeContext == null)
                throw new ArgumentNullException(nameof(runtimeContext));
            if (!string.IsNullOrWhiteSpace(uri))
                throw new ArgumentOutOfRangeException(nameof(uri));
            if (!string.IsNullOrWhiteSpace(node))
                throw new ArgumentOutOfRangeException(nameof(node));

            this.runtimeContext = runtimeContext;
            this.address = new Address(uri);
            this.node = node;
        }

        public void Dispose()
        {
            disposed = true;

            if (senderDisposable != null)
            {
                senderDisposable.Dispose();
                senderDisposable = null;
            }

            if (senderLink != null)
            {
                senderLink.Closed -= OnClosed;
                Session session = senderLink.Session;
                session.Closed -= OnClosed;
                senderLink.Close();
                session.Close();
                senderLink = null;
                session = null;
            }

            foreach (Tuple<ReceiverLink, IObserver<IPipelineTransportContext>> receiver in receivers)
            {
                ReceiverLink receiverLink = receiver.Item1;
                if (receiverLink != null)
                {
                    receiverLink.Closed -= OnClosed;
                    Session session = receiverLink.Session;
                    session.Closed -= OnClosed;
                    receiverLink.Close();
                    session.Close();
                }

                IObserver<IPipelineTransportContext> observer = receiver.Item2;
                if (observer != null)
                    observer.OnCompleted();
            }

            receivers.Clear();

            if (connection != null)
            {
                connection.Closed -= OnClosed;
                connection.Close();
                connection = null;
            }
        }

        public void OnCompleted()
        {
            if (disposed)
                return;

            if (senderDisposable != null)
            {
                senderDisposable.Dispose();
                senderDisposable = null;
            }

            if (senderLink != null)
            {
                senderLink.Closed -= OnClosed;
                Session session = senderLink.Session;
                session.Closed -= OnClosed;
                senderLink.Close();
                session.Close();
                senderLink = null;
                session = null;
            }
        }

        public void OnError(Exception error)
        {
            if (disposed || error == null)
                return;

            runtimeContext.Log.Error(error);

            if (senderDisposable != null)
            {
                senderDisposable.Dispose();
                senderDisposable = null;
            }

            if (senderLink != null)
            {
                senderLink.Closed -= OnClosed;
                Session session = senderLink.Session;
                session.Closed -= OnClosed;
                senderLink.Close();
                session.Close();
                senderLink = null;
                session = null;
            }
        }

        public void OnNext(IPipelineTransportContext value)
        {
            if (disposed || value == null || value.Message == null || value.Message.Body == null)
                return;

            Stream stream = value.Message.Body;
            stream.Position = 0;

            if (PrepareConnection())
            {
                if (senderLink == null || senderLink.IsClosed || senderLink.Session.IsClosed)
                {
                    Session session = new Session(connection);
                    senderLink = new SenderLink(session, "x-neurocita-reactive-senderlink", node);
                    using (Message message = new Message())
                    {
                        // ToDo: Implement header values
                        // ToDo: Implement ByteBuffer
                        message.BodySection = new Data() { Binary = new byte[stream.Length] };
                        stream.Read((message.BodySection as Data).Binary, 0, (int) stream.Length);
                        senderLink.SendAsync(message).Wait();
                    }
                }
            }
        }

        public IDisposable Subscribe(IObserver<IPipelineTransportContext> observer)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(AmqpTransport));

            if (PrepareConnection())
            {
                Session session = new Session(connection);
                ReceiverLink receiverLink = new ReceiverLink(session, "x-neurocita-reactive-receiverlink", node);
                receiverLink.AddClosedCallback(OnClosed);
                receiverLink.Start(1, OnMessage);
                receivers.Add(Tuple.Create(receiverLink, observer));
            }
            throw new NotImplementedException();
        }

        private bool CanCloseConnection()
        {
            ClosedCallback closedCallback = OnClosed;
            return closedCallback.GetInvocationList().Length == 0;
        }

        private bool PrepareConnection()
        {
            while (!disposed && (connection == null || connection.IsClosed))
            {
                try
                {
                    connection = Connection.Factory.CreateAsync(address).Result;
                    connection.AddClosedCallback(OnClosed);
                }
                catch (AmqpException exception)
                {
                    runtimeContext.Log.Error(exception);
                    // ToDo:Check exception type
                    Task.Delay(1000).Wait();    // Only for reconnect
                }
            }

            return connection != null && !connection.IsClosed;
        }

        private void OnClosed(IAmqpObject sender, global::Amqp.Framing.Error error)
        {
            if (disposed)
                return;

            ReceiverLink receiverLink = sender as ReceiverLink;
            if (receiverLink != null)
            {
                if (connection.IsClosed)
                    return;

                receiverLink.Closed -= OnClosed;
                receiverLink.Session.Closed -= OnClosed;
                receiverLink.Session.Close();
                Tuple<ReceiverLink, IObserver<IPipelineTransportContext>> receiver = receivers.Where(tuple => tuple.Item1 == receiverLink).FirstOrDefault();
                IObserver<IPipelineTransportContext> observer = receiver.Item2;
                receivers.Remove(receiver);

                if (PrepareConnection())
                {
                    Session session = new Session(connection);
                    receiverLink = new ReceiverLink(session, "x-neurocita-reactive-receiverlink", node);
                    receiverLink.AddClosedCallback(OnClosed);
                    receiverLink.Start(1, OnMessage);
                    receivers.Add(Tuple.Create(receiverLink, observer));
                }

                return;
            }

            if (connection == sender)
            {
                if (PrepareConnection())
                {

                    foreach (Tuple<ReceiverLink, IObserver<IPipelineTransportContext>> receiver in receivers)
                    {
                        receiverLink = receiver.Item1;
                        if (receiverLink != null)
                        {
                            receiverLink.Closed -= OnClosed;
                            Session session = receiverLink.Session;
                            session.Closed -= OnClosed;
                            receiverLink.Close();
                            session.Close();
                        }

                        IObserver<IPipelineTransportContext> observer = receiver.Item2;
                        receivers.Remove(receiver);

                        Session session = new Session(connection);
                        receiverLink = new ReceiverLink(session, "x-neurocita-reactive-receiverlink", node);
                        receiverLink.AddClosedCallback(OnClosed);
                        receiverLink.Start(1, OnMessage);
                        receivers.Add(Tuple.Create(receiverLink, observer));
                    }
                }
            }
        }

        private void OnMessage(IReceiverLink receiver, Message message)
        {
            // IRuntimeContext
            // IMessage<Stream> message
            // ITransportPipelineContext context = new TransportRuntimeContext;
            // ...
            //OnNext(context);

            throw new NotImplementedException();
        }
    }
#endif
}
