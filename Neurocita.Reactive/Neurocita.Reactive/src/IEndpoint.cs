using System;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive
{
    public interface IEndpoint
    {
        string NodePath { get; }
        ISourceEndpoint AsSource();
        ISinkEndpoint AsSink();
    }
}
    