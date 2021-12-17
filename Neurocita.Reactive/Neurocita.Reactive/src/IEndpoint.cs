using System;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive
{
    public interface IEndpoint : INode, IDisposable
    {
        ISourceEndpoint AsSource();
        ISinkEndpoint AsSink();
    }
}
    