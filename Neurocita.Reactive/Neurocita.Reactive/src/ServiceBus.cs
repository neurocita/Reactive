using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using Neurocita.Reactive.Configuration;
using Neurocita.Reactive.Transport;
using Neurocita.Reactive.Serialization;

namespace Neurocita.Reactive
{
    public class ServiceBus : IServiceBus
    {
        private bool disposed = false;
        private readonly object mutex = new object();
        private readonly ITransport transport;
        private readonly ISerializer serializer;
        private readonly IServiceBusConfiguration configuration;
        private readonly IReadOnlyDictionary<string, IEndpoint> endpoints;

        public IServiceBusConfiguration Configuration { get; }
        public IReadOnlyDictionary<string, IEndpoint> Endpoints => endpoints;

        public ServiceBus(IServiceBusConfiguration configuration)
        {
            this.configuration = configuration;
            IDictionary<string, IEndpoint> endpoints = new Dictionary<string, IEndpoint>();
            foreach (var endpointConfiguration in this.configuration.Endpoints)
            {
                Endpoint endpoint = new Endpoint(configuration.Transport.Factory.Create(), configuration.Serialization.Factory.Create(), endpointConfiguration.Value.NodePath);
                endpoints.Add(endpointConfiguration.Key, endpoint);
            }
        }

        internal ServiceBus(ITransport transport, ISerializer serializer, IDictionary<string, IEndpoint> endpoints)
        {
            this.transport = transport;
            this.serializer = serializer;
            this.endpoints = new ReadOnlyDictionary<string, IEndpoint>(endpoints);
        }
        
        public static IServiceBusBuilder Configure()
        {
            return new ServiceBusBuilder();
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                lock (mutex)
                {
                    new CompositeDisposable(endpoints.Values).Dispose();
                    disposed = true;
                }

        }
    }
}
