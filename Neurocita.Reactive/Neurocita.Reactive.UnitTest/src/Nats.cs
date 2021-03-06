using Microsoft.VisualStudio.TestTools.UnitTesting;
using NATS.Client;
using NATS.Client.Rx;
using Neurocita.Reactive;
using Neurocita.Reactive.Configuration;
using Neurocita.Reactive.Pipeline;
using Neurocita.Reactive.Serialization;
using Neurocita.Reactive.Transport;
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
        public void TestSimpleConnect()
        {
            var connectionFactory = new ConnectionFactory();
            using (IConnection connection = connectionFactory.CreateConnection(url))
            {
                using (IAsyncSubscription subscription = connection.SubscribeAsync(node))
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
        public void TestRxConnect()
        {
            var connectionFactory = new NATS.Client.ConnectionFactory();
            using (IConnection connection = connectionFactory.CreateConnection(url))
            {
                using (INATSObservable<Msg> observable = connection.Observe(node))
                {
                    using (observable.Subscribe(msg => Console.WriteLine(BitConverter.ToInt32(msg.Data, 0))))
                    {
                        using (IConnection connection2 = connectionFactory.CreateConnection(url))
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
        public void TestPipeline()
        {
            ITransport transport = new NatsTransportFactory().Create();
            ISerializer serializer = new DataContractJsonSerializerFactory().Create();
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
            {
                using (ITransportMessageSource transportMessageSource = transport.CreateSource(node))
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
                        using (ITransportMessageSink transportMessageSink = transport.CreateSink(node))
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
                                    Task.Delay(TimeSpan.FromSeconds(20), cancellationTokenSource.Token).Wait();
                                }
                                catch (AggregateException exception)
                                {
                                    if (exception.InnerException is TaskCanceledException)
                                        Console.WriteLine("Sequence completed");
                                    else
                                        throw exception.InnerException;
                                }
                            }
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void TestServiceBus()
        {
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
            {
                using (IServiceBus serviceBus = ServiceBus
                    .Configure()
                        //.Over
                        //.As
                        .TransportWithNats()
                        .SerializeWithBinary()
                        .WithEndpoint("test", "test")
                    .Create())
                {
                    using (var source = serviceBus.Endpoints["test"].AsSource())
                    {
                        using (source
                            .AsObservable<ValueTypeDataContract<int>>()
                            .Subscribe(
                                value => Console.WriteLine(value.Value),
                                exception => throw exception,
                                () => cancellationTokenSource.Cancel()))
                        {
                            using (var sink = serviceBus.Endpoints["test"].AsSink())
                            {
                                using (sink.From(Observable.Range(1, 20).ToDataContract()))
                                {
                                    try
                                    {
                                        Task.Delay(TimeSpan.FromSeconds(20), cancellationTokenSource.Token).Wait();
                                    }
                                    catch (AggregateException exception)
                                    {
                                        if (exception.InnerException is TaskCanceledException)
                                            Console.WriteLine("Sequence completed");
                                        else
                                            throw exception.InnerException;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
