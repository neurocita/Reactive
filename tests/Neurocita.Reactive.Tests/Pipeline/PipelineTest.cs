using System.Reactive;
using System.Reactive.Linq;
using Neurocita.Reactive.Serialization;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive.Tests;

public class PipelineTest
{
    const string node = "test";
    ISerializer serializer = DataContractSerializer.Xml();

    public PipelineTest()
    {

    }

    [Fact]
    public void LowLevelReceiverWithSenderTest()
    {
        using (var transport = InMemoryTransport.PubSub())
        {
            using (var receiver = RemoteObservable.From(transport, node)
                                                .Do((message) => Console.WriteLine(message.Body == null ? "<null>" : new StreamReader(message.Body).ReadToEnd()))
                                                .Unpack<int>(serializer)
                                                .Unwrap()
                                                .Subscribe((value) => Assert.InRange(value, 1, 10)))
            {
                using (var sender = Observable.Range(1, 10)
                                            .Wrap(new Dictionary<string,object>())
                                            .Pack(serializer)
                                            .SendTo(transport, node))
                {
                    Task.Delay(500);
                }

                Task.Delay(500);
            }
        }
    }

    [Fact]
    public void LowLevelSenderWithReceiverTest()
    {
        using (var transport = InMemoryTransport.P2P())
        {
            using (var sender = Observable.Range(1, 10)
                                        .Wrap(new Dictionary<string,object>())
                                        .Pack(serializer)
                                        .SendTo(transport, node))
            {
                using (var receiver = RemoteObservable.From(transport, node)
                                                    .Do((message) => Console.WriteLine(message.Body == null ? "<null>" : new StreamReader(message.Body).ReadToEnd()))
                                                    .Unpack<int>(serializer)
                                                    .Unwrap()
                                                    .Subscribe((value) => Assert.InRange(value, 1, 10)))
                {
                    Task.Delay(500);
                }

                Task.Delay(500);
            }
        }
    }
}