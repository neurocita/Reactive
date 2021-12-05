using System;
using System.Collections.Generic;
using System.Text;

namespace Neurocita.Reactive
{
    public class ServiceBusConfiguration : IServiceBusConfiguration
    {
        public IServiceBusBuilder CreateBuilder()
        {
            
            return new ServiceBusBuilder();
        }
    }
}
