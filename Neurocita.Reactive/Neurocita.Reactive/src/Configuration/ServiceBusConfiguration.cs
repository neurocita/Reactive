using System.Collections.Generic;

namespace Neurocita.Reactive.Configuration
{
    public class ServiceBusConfiguration : IServiceBusConfiguration
    {
        private TransportConfiguration transportConfiguration;
        private SerializerConfiguration serializationConfiguration;
        private ILoggingConfiguration loggingConfiguration;
        private readonly IDictionary<string, IEndpointConfiguration> endpointConfigurations = new Dictionary<string, IEndpointConfiguration>();

        //public static ITransportConfiguration DefaultTransport = new InMemoryTransportConfiguration();
        //public static ISerializationConfiguration DefaultSerialization = new BinarySerializerConfiguration();
        public static ILoggingConfiguration DefaultLogging = new NullLoggerConfiguration();

        public ServiceBusConfiguration()
        {
            //this.Transport = DefaultTransport;
            //this.Serialization = DefaultSerialization;
            this.Logging = DefaultLogging;
        }

        public TransportConfiguration Transport
        {
            get { return transportConfiguration; }
            internal set { transportConfiguration = value; }
        }
        public SerializerConfiguration Serialization
        {
            get { return serializationConfiguration; }
            internal set { serializationConfiguration = value; }
        }

        public ILoggingConfiguration Logging
        {
            get { return loggingConfiguration; }
            internal set { loggingConfiguration = value; }
        }

        public IDictionary<string, IEndpointConfiguration> Endpoints => endpointConfigurations;

        public IServiceBusConfiguration AddEndpoint(string name, string nodePath)
        {
            this.endpointConfigurations.Add(name, new EndpointConfiguration(nodePath));
            return this;
        }

        public IServiceBus Create()
        {
            return new ServiceBus(this);
        }
    }
}
