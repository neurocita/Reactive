using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public interface IServiceBus : IDisposable
    {
        IReadOnlyDictionary<string, IEndpoint> Endpoints { get; }
    }
}
