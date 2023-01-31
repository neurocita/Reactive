using System.Collections.Generic;
using Neurocita.Reactive.Transport;
using Neurocita.Reactive.Serialization;

namespace Neurocita.Reactive.Configuration
{
    public interface IServiceBusConfiguration
    {
        /*
        ITransportConfiguration TransferUsing { get; }
        ISerializationConfiguration FormatAs { get; }
        ILoggingConfiguration LogWith { get; }
        */
        IComponentConfiguration<ITransport> Transport { get; }
        IComponentConfiguration<ISerializer> Serializer { get; }
        IDictionary<string, IEndpointConfiguration> Endpoints { get; }
    }
}
