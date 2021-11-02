using System;

namespace Neurocita.Reactive
{
    public interface IPipelineSubscriber : IDisposable
    {
        string Address { get; }
    }
}
