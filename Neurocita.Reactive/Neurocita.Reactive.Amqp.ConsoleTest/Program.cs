using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amqp;
using Amqp.Handler;
using Amqp.Framing;

namespace Neurocita.Reactive.Amqp.ConsoleTest
{
    class Program
    {
        static string queueName = "Test";

        static void Main(string[] args)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            Task runnable = Task.Run(() => Run(autoResetEvent));

            Console.WriteLine("Press any key to close connection...");
            Console.ReadKey();
            autoResetEvent.Set();

            Console.WriteLine("Press any key end...");
            Console.ReadKey();

            runnable.Wait();
        }

        static async void Run(AutoResetEvent autoResetEvent)
        {
            //IConnectionFactory connectionFactory = Connection.Factory;

            Address address = new Address("amqp://localhost:5672");
            IConnection connection = await Connection.Factory.CreateAsync(address);
            connection.AddClosedCallback(OnClosed);

            ISession session1 = connection.CreateSession();
            session1.AddClosedCallback(OnClosed);
            Message message = new Message("Hello AMQP");
            ISenderLink sender = session1.CreateSender("link", queueName);
            sender.AddClosedCallback(OnClosed);
            await sender.SendAsync(message);

            ISession session2 = connection.CreateSession();
            session2.AddClosedCallback(OnClosed);
            IReceiverLink receiver = session2.CreateReceiver("link", queueName);
            receiver.AddClosedCallback(OnClosed);
            receiver.Start(1, OnMessage);
            Task.Delay(1000).Wait();
            autoResetEvent.WaitOne();
            /*
            if (!sender.IsClosed)
                await sender.CloseAsync();
            if (!receiver.IsClosed)
                await receiver.CloseAsync();

            if (!session.IsClosed)
                await session.CloseAsync();
            */
            if (!connection.IsClosed)
                try
                {
                    await connection.CloseAsync();  // amqp:illegal-state
                }
                catch (AmqpException exception)
                {
                    Console.WriteLine(connection.Error.Condition);
                    Console.WriteLine(exception.Message);
                }
        }

        private static void OnClosed(IAmqpObject sender, Error error)
        {
            Console.WriteLine($"Instance of type {sender.GetType().Name} closed.");

            if (sender is Link)
            {
                Console.WriteLine($"Link state: {(sender as Link).LinkState}");
                Console.WriteLine($"Session is closed: {(sender as Link).Session.IsClosed}");
                Console.WriteLine($"Connection is closed: {(sender as Link).Session.Connection.IsClosed}");
            }
            else if (sender is Session)
            {
                Console.WriteLine($"Session state: {(sender as Session).SessionState}");
                Console.WriteLine($"Connection is closed: {(sender as Session).Connection.IsClosed}");
            }
            else if (sender is Connection)
            {
                Console.WriteLine($"Connection state: {(sender as Connection).ConnectionState}");
            }

            if (error != null)
            {
                Console.WriteLine($"Error condition: {error.Condition}"); // Close requested: amqp:link:detach-forced, Connection interrupted: amqp:connection:forced
                Console.WriteLine($"Error description: {error.Description}");
            }
        }

        private static void OnMessage(IReceiverLink receiver, Message message)
        {
            Console.WriteLine(message.GetBody<string>());
        }
    }
}
