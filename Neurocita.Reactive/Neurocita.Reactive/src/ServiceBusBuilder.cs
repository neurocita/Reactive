using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Neurocita.Reactive
{
    internal class ServiceBusBuilder : IServiceBusBuilder
    {
        public IServiceBus Build()
        {
            // ToDo: Just a place holder - implement!!!
            IDictionary<string, IEndpoint> endpoints = new Dictionary<string, IEndpoint>();
            return new ServiceBus(endpoints);
        }
    }
}
