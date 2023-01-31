using Neurocita.Reactive.Configuration;
using Neurocita.Reactive.Serialization;
using Neurocita.Reactive.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neurocita.Reactive
{
    internal class ServiceBusBuilder : IServiceBusBuilder
    {
        public IComponentConfiguration<ITransport> Transport => throw new NotImplementedException();

        public IComponentConfiguration<ISerializer> Serializer => throw new NotImplementedException();
        public IDictionary<string,IEndpoint> Endpoints => throw new NotImplementedException();

        public IServiceBus Build()
        {
            ITransport transport = Transport.Factory.Invoke();
            ISerializer serializer = Serializer.Factory.Invoke();
            return new ServiceBus(transport, serializer, Endpoints);
        }
    }
}
