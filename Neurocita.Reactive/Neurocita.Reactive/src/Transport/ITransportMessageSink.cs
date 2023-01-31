using System;
using System.IO;

namespace Neurocita.Reactive.Transport
{
    public interface ITransportMessageSink : IDisposable
    {
        string NodePath { get; }
        IDisposable Observe(IObservable<IMessage<Stream>> messages);
    }
}
