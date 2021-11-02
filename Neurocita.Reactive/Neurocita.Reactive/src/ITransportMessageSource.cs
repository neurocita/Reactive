using System;
using System.IO;

namespace Neurocita.Reactive
{
    public interface ITransportMessageSource : IDisposable
    {
        string Address { get; }
        IObservable<IMessage<Stream>> Messages { get; }
    }
}
