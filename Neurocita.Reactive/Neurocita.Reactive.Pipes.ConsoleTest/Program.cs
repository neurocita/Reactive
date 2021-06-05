using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurocita.Reactive.Pipes
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine();
            Console.WriteLine("Test consumer server with producers created/disposed first");
            Console.WriteLine("==========================================================");
            using (var consumer = PipeStreamObservable.FromPipeStream<string>("test"))
            {
                using (var producer1 = Observable.Interval(TimeSpan.FromMilliseconds(500))
                                                .Select<long, string>(value => $"Producer 1: {value}")
                                                .ToPipeStream(".", "test"))
                using (var producer2 = Observable.Interval(TimeSpan.FromMilliseconds(500))
                                                .Select<long, string>(value => $"Producer 2: {value}")
                                                .ToPipeStream(".", "test"))
                using (var producer3 = Observable.Interval(TimeSpan.FromMilliseconds(500))
                                                .Select<long, string>(value => $"Producer 3: {value}")
                                                .ToPipeStream(".", "test"))
                {
                    using (var observer = consumer.Subscribe(value => Console.WriteLine(value)))
                    {

                        Console.WriteLine("Press any key to stop observer...");
                        Console.ReadKey();
                    }

                    Console.WriteLine("Press any key to stop producers...");
                    Console.ReadKey();
                }

                Console.WriteLine("Press any key to stop consumer...");
                Console.ReadKey();
            }
            
            Console.WriteLine();
            Console.WriteLine("Test consumer server with producers created/disposed last");
            Console.WriteLine("=========================================================");
            using (var consumer = PipeStreamObservable.FromPipeStream<string>("test"))
            {
                using (var observer = consumer.Subscribe(value => Console.WriteLine(value)))
                {
                    using (var producer1 = Observable.Interval(TimeSpan.FromMilliseconds(500))
                                                    .Select<long, string>(value => $"Producer 1: {value}")
                                                    .ToPipeStream(".", "test"))
                    using (var producer2 = Observable.Interval(TimeSpan.FromMilliseconds(500))
                                                    .Select<long, string>(value => $"Producer 2: {value}")
                                                    .ToPipeStream(".", "test"))
                    using (var producer3 = Observable.Interval(TimeSpan.FromMilliseconds(500))
                                                    .Select<long, string>(value => $"Producer 3: {value}")
                                                    .ToPipeStream(".", "test"))
                    {
                        Console.WriteLine("Press any key to stop producers...");
                        Console.ReadKey();
                    }

                    Console.WriteLine("Press any key to stop observer...");
                    Console.ReadKey();
                }

                Console.WriteLine("Press any key to stop consumer...");
                Console.ReadKey();
            }
            */
            /*
            Console.WriteLine();
            Console.WriteLine("Test producer server with consumers created/disposed last");
            Console.WriteLine("=========================================================");
            using (var producer = Observable.Interval(TimeSpan.FromMilliseconds(500))
                                            .ToPipeStream("test"))
            {
                using (var consumer = PipeStreamObservable.FromPipeStream<long>(".", "test"))
                {
                    using (var observer1 = consumer.Subscribe(value => Console.WriteLine($"Observer 1: {value}")))
                    using (var observer2 = consumer.Subscribe(value => Console.WriteLine($"Observer 2: {value}")))
                    using (var observer3 = consumer.Subscribe(value => Console.WriteLine($"Observer 3: {value}")))
                    {
                        Console.WriteLine("Press any key to stop observer 1...");
                        Console.ReadKey();
                        observer1.Dispose();

                        Console.WriteLine("Press any key to stop observer 2...");
                        Console.ReadKey();
                        observer2.Dispose();

                        Console.WriteLine("Press any key to stop observer 3...");
                        Console.ReadKey();
                        observer3.Dispose();
                    }

                    Console.WriteLine("Press any key to stop consumer...");
                    Console.ReadKey();
                }

                Console.WriteLine("Press any key to stop producer...");
                Console.ReadKey();
            }
            */
            Console.WriteLine();
            Console.WriteLine("Test producer server with consumers created/disposed last");
            Console.WriteLine("=========================================================");
            using (var producer = Observable.Timer(DateTimeOffset.UtcNow.AddSeconds(5), TimeSpan.FromMilliseconds(500))
                                            .Take(10)
                                            .ToPipeStream("test"))
            {
                using (var consumer1 = PipeStreamObservable.FromPipeStream<long>(".", "test"))
                using (var consumer2 = PipeStreamObservable.FromPipeStream<long>(".", "test"))
                using (var consumer3 = PipeStreamObservable.FromPipeStream<long>(".", "test"))
                {
                    using (var observer1 = consumer1.Subscribe(value => Console.WriteLine($"Observer 1: {value}")))
                    using (var observer2 = consumer2.Subscribe(value => Console.WriteLine($"Observer 2: {value}")))
                    using (var observer3 = consumer3.Subscribe(value => Console.WriteLine($"Observer 3: {value}")))
                    {
                        Console.WriteLine("Press any key to stop observer 1...");
                        Console.ReadKey();
                        observer1.Dispose();

                        Console.WriteLine("Press any key to stop observer 2...");
                        Console.ReadKey();
                        observer2.Dispose();

                        Console.WriteLine("Press any key to stop observer 3...");
                        Console.ReadKey();
                        observer3.Dispose();
                    }

                    Console.WriteLine("Press any key to stop consumer...");
                    Console.ReadKey();
                }

                Console.WriteLine("Press any key to stop producer...");
                Console.ReadKey();
            }
        }
    }
}
