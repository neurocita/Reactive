using System;
using System.IO;

namespace Neurocita.Reactive
{
    public interface ITransportMessageSource : IDisposable
    {
        string Node { get; }
        IObservable<IMessage<Stream>> Messages { get; }
    }
}
