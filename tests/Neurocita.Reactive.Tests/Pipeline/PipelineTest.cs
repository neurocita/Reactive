using System.Reactive.Linq;
using Neurocita.Reactive.Serialization;
using Neurocita.Reactive.Transport;
using Xunit.Abstractions;

namespace Neurocita.Reactive.Tests;

public class TransportSerializerData : TheoryData<InMemoryTransport,DataContractSerializer>
{
    public TransportSerializerData()
    {
        Add(InMemoryTransport.PubSub, DataContractSerializer.Json);
        Add(InMemoryTransport.PubSub, DataContractSerializer.Xml);
        Add(InMemoryTransport.P2P, DataContractSerializer.Json);
        Add(InMemoryTransport.P2P, DataContractSerializer.Xml);
    }
}

public class PipelineTest : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly string _node = "test";
    private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    private readonly int timeout = 2000;
    private int received = 0, send = 0;

    public PipelineTest(ITestOutputHelper output)
    {
        _output = output;
    }

    public void Dispose()
    {
        cancellationTokenSource.Dispose();
    }

    [Theory]
    [ClassData(typeof(TransportSerializerData))]
    public void ReceiverSenderTest(InMemoryTransport transport, DataContractSerializer serializer)
    {
        _output.WriteLine("Function: {0}", nameof(ReceiverSenderTest));
        _output.WriteLine("Transport: {0}, Serialization: {1}", transport.ExchangePattern, serializer.Format);
        _output.WriteLine("==========================================================================");

        using (transport)
        {
            using (var receiver = RemoteObservable.From(transport, _node)
                                                //.Do((message) => _output.WriteLine(message.Body == null ? "<null>" : new StreamReader(message.Body).ReadToEnd()))
                                                .Unpack<int>(serializer)
                                                .Unwrap()
                                                .Do(value => received++)
                                                .Subscribe(value => Assert.InRange(value, 1, 10)
                                                    	    , exception => _output.WriteLine(exception.Message)
                                                            , () => cancellationTokenSource.Cancel()))
            {
                using (var sender = Observable.Range(1, 10)
                                            .Do(value => send++)
                                            .Wrap(new Dictionary<string,object>())
                                            .Pack(serializer)
                                            .SendTo(transport, _node))
                {
                    try
                    {
                        Task.Delay(timeout, cancellationTokenSource.Token).Wait();
                    }
                    catch (AggregateException exception)
                    {
                        if (exception
                                .InnerExceptions
                                .Select(ex => ex.GetType())
                                .Contains(typeof(TaskCanceledException)))
                            _output.WriteLine("Sender successfully cancelled.");
                        else
                            throw exception;
                    }
                }
            
                try
                {
                    Task.Delay(timeout, cancellationTokenSource.Token).Wait();
                }
                catch (AggregateException exception)
                {
                    if (exception
                            .InnerExceptions
                            .Select(ex => ex.GetType())
                            .Contains(typeof(TaskCanceledException)))
                        _output.WriteLine("Receiver successfully cancelled.");
                    else
                        throw exception;
                }
            }
        }

        _output.WriteLine("Send: {0}, received: {1}", send, received);
        Assert.True(send == received);
    }

    [Theory]
    [ClassData(typeof(TransportSerializerData))]
    public void SenderReceiverTest(InMemoryTransport transport, DataContractSerializer serializer)
    {
        _output.WriteLine("Function: {0}", nameof(SenderReceiverTest));
        _output.WriteLine("Transport: {0}, Serialization: {1}", transport.ExchangePattern, serializer.Format);
        _output.WriteLine("==========================================================================");

        using (transport)
        {
            using (var sender = Observable.Range(1, 10)
                                        .Do(value => send++)
                                        .Wrap(new Dictionary<string,object>())
                                        .Pack(serializer)
                                        .SendTo(transport, _node))
            {
                using (var receiver = RemoteObservable.From(transport, _node)
                                                    //.Do((message) => _output.WriteLine(message.Body == null ? "<null>" : new StreamReader(message.Body).ReadToEnd()))
                                                    .Unpack<int>(serializer)
                                                    .Unwrap()
                                                    .Do(value => received++)
                                                    .Subscribe(value => Assert.InRange(value, 1, 10)
                                                        	    , exception => _output.WriteLine(exception.Message)
                                                                , () => cancellationTokenSource.Cancel()))
                {
                    try
                    {
                        Task.Delay(timeout, cancellationTokenSource.Token).Wait();
                    }
                    catch (AggregateException exception)
                    {
                        if (exception
                                .InnerExceptions
                                .Select(ex => ex.GetType())
                                .Contains(typeof(TaskCanceledException)))
                            _output.WriteLine("Receiver successfully cancelled.");
                        else
                            throw exception;
                    }
                }

                try
                {
                    Task.Delay(timeout, cancellationTokenSource.Token).Wait();
                }
                catch (AggregateException exception)
                {
                    if (exception
                            .InnerExceptions
                            .Select(ex => ex.GetType())
                            .Contains(typeof(TaskCanceledException)))
                        _output.WriteLine("Sender successfully cancelled.");
                    else
                        throw exception;
                }
            }
        }

        _output.WriteLine("Send: {0}, received: {1}", send, received);
        if (transport.ExchangePattern == InMemoryExchangePattern.PointToPoint)
            Assert.True(send == received);
        else
            Assert.False(send == received);
    }
}