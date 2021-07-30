using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public interface IServiceConfiguration
    {
        IServiceBusConfiguration ServiceBus { get; }
        IReadOnlyCollection<IEndpointConfiguration> Endpoints { get; }
    }
}
