using System;
using System.IO;

namespace Neurocita.Reactive.Transport
{
    public interface ITransportMessageSource : INode, IDisposable
    {
        IObservable<IMessage<Stream>> Messages { get; }
    }
}
