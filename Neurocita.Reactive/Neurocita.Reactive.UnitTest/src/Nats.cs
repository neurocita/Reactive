using Microsoft.VisualStudio.TestTools.UnitTesting;
using NATS.Client;
using NATS.Client.Rx;
using Neurocita.Reactive.Nats;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Neurocita.Reactive.UnitTest
{
    [TestClass]
    public class Nats
    {
        private const string host = "localhost";
        private const string port = "4222";
        private const string user = "ruser";
        private const string password = "T0pS3cr3t";
        private const string node = "test";
        private readonly string url;

        public Nats()
        {
            url = $"{user}:{password}@{host}:{port}";
        }

        [TestMethod]
        public void SimpleConnect()
        {
            var connectionFactory = new ConnectionFactory();
            using (var connection = connectionFactory.CreateConnection(url))
            {
                using (var subscription = connection.SubscribeAsync(node))
                {
                    subscription.MessageHandler += Subscription_MessageHandler;
                    subscription.Start();

                    for (int counter = 1; counter <= 10; counter++)
                    {
                        Msg msg = new Msg
                        {
                            Subject = node,
                            Data = BitConverter.GetBytes(counter)
                        };
                        connection.Publish(msg);
                    }
                    connection.Flush();

                    Task.Delay(5000).Wait();

                    subscription.Unsubscribe();
                }
                connection.Drain();
                connection.Close();
            }
        }

        private void Subscription_MessageHandler(object sender, MsgHandlerEventArgs e)
        {
            Console.WriteLine(BitConverter.ToInt32(e.Message.Data, 0));
        }

        [TestMethod]
        public void RxConnect()
        {
            var connectionFactory = new NATS.Client.ConnectionFactory();
            using (var connection = connectionFactory.CreateConnection(url))
            {
                using (var observable = connection.Observe(node))
                {
                    using (observable.Subscribe(msg => Console.WriteLine(BitConverter.ToInt32(msg.Data, 0))))
                    {
                        using (var connection2 = connectionFactory.CreateConnection(url))
                        {
                            for (int counter = 1; counter <= 10; counter++)
                            {
                                Msg msg = new Msg
                                {
                                    Subject = node,
                                    Data = BitConverter.GetBytes(counter)
                                };
                                connection2.Publish(msg);
                            }
                            connection2.Flush();
                            connection2.Close();
                        }

                        Task.Delay(5000).Wait();
                    }
                }

                connection.Drain();
                connection.Close();
            }
        }

        [TestMethod]
        public void Pipeline()
        {
            ITransportMessageFactory transportMessageFactory = new NatsMessageFactory();
            ISerializer serializer = new DataContractJsonSerializerFactory().CreateSerializer();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
            {
                using (ITransportMessageSource transportMessageSource = transportMessageFactory.CreateSource(node))
                {
                    using (PipelineObservable.Create(transportMessageSource)
                        .ToPipelineContext()
                        .Deserialize<ValueTypeDataContract<int>>(serializer)
                        .ToDataContract()
                        .ToValue()
                        //.ObserveOn(new EventLoopScheduler())
                        .Subscribe(value => Console.WriteLine(value)
                                    , exception => Console.WriteLine(exception)
                                    , () => cancellationTokenSource.Cancel()))
                    {
                        using (ITransportMessageSink transportMessageSink = transportMessageFactory.CreateSink(node))
                        {
                            using (transportMessageSink.Observe(Observable.Range(1, 10)
                                //.ObserveOn(new EventLoopScheduler())
                                //.Delay(TimeSpan.FromSeconds(1))
                                .ToDataContract()
                                .ToPipelineContext()
                                .Serialize(serializer)
                                .ToTransportMessage()))
                            {
                                try
                                {
                                    Task.Delay(TimeSpan.FromSeconds(60), cancellationTokenSource.Token).Wait();
                                }
                                catch (AggregateException exception)
                                {
                                    Console.WriteLine(exception);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
