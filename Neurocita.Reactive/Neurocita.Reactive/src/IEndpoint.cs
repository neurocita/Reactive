using System;

namespace Neurocita.Reactive
{
    public interface IEndpoint : INode, IDisposable
    {
        ISourceEndpoint AsSource();
        ISinkEndpoint AsSink();
    }
}
    