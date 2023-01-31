using System;

namespace Neurocita.Reactive.Configuration
{
    public static class NullLoggerConfigurationExtensions
    {
        public static IServiceBusConfiguration Silent(this NullLoggerConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (configuration.ServiceBusConfiguration == null)
                throw new ArgumentNullException(nameof(configuration.ServiceBusConfiguration));

            return configuration.ServiceBusConfiguration;
        }
    }
}
