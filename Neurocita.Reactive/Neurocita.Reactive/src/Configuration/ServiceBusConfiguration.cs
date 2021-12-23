using System;
using System.Collections.Generic;
using Neurocita.Reactive.Transport;
using Neurocita.Reactive.Serialization;

namespace Neurocita.Reactive.Configuration
{
    public class ServiceBusConfiguration : IServiceBusConfiguration
    {
        private ITransportFactory transportFactory;
        private ISerializerFactory serializerFactory;
        private readonly IDictionary<string, IEndpointConfiguration> endpointConfigurations = new Dictionary<string, IEndpointConfiguration>();

        public ICanAddSerializer WithTransport(ITransportFactory transportFactory)
        {
            this.transportFactory = transportFactory;
            return this;
        }

        public ICanAddEndpoint WithSerializer(ISerializerFactory serializerFactory)
        {
            this.serializerFactory = serializerFactory;
            return this;
        }

        public ICanAddEndpointOrCreate WithEndpoint(string name, string nodePath)
        {
            this.endpointConfigurations.Add(name, new EndpointConfiguration(nodePath));
            return this;
        }

        public IServiceBus Create()
        {
            IDictionary<string, IEndpoint> endpoints = new Dictionary<string, IEndpoint>();
            foreach (var endpointConfiguration in endpointConfigurations)
            {
                Endpoint endpoint = new Endpoint(transportFactory.Create(), serializerFactory.Create(), endpointConfiguration.Value.NodePath);
                endpoints.Add(endpointConfiguration.Key, endpoint);
            }
            return new ServiceBus(endpoints);
        }
    }
}
