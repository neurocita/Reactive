using System.Collections.Generic;

namespace Neurocita.Reactive.Configuration
{
    public interface IServiceBusConfiguration : ICanAddTransport, ICanAddSerializer, ICanAddEndpointOrBuild
    {

    }
}
