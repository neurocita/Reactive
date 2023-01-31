using System;
using System.IO;

namespace Neurocita.Reactive.Transport
{
    public interface ITransportMessageSource : IDisposable
    {
        string NodePath { get; }
        IObservable<IMessage<Stream>> Messages { get; }
    }
}
