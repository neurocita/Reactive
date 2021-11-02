using System;
using System.IO;

namespace Neurocita.Reactive
{
    public interface ITransportMessageSink : IDisposable
    {
        string Address { get; }
        IObserver<IMessage<Stream>> Sink { get; }
    }
}
