namespace Neurocita.Reactive.Configuration
{
    public class NullLoggerConfiguration : ILoggingConfiguration
    {
        public NullLoggerConfiguration()
        {
        }

        internal NullLoggerConfiguration(IServiceBusConfiguration serviceBusConfiguration)
        {
            ServiceBusConfiguration = serviceBusConfiguration;
        }

        public ILoggerFactory Factory => new NullLoggerFactory();
        internal IServiceBusConfiguration ServiceBusConfiguration { get; }
    }
}
