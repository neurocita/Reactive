using System;
using System.IO;

namespace Neurocita.Reactive
{
    public interface ITransportMessageSource : INode, IDisposable
    {
        IObservable<IMessage<Stream>> Messages { get; }
    }
}
