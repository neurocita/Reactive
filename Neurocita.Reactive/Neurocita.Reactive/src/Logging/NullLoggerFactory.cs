using Neurocita.Reactive.Configuration;
using System;

namespace Neurocita.Reactive.Logging
{
    public sealed class NullLoggerFactory : ILoggerFactory
    {
        public static ILoggerFactory Instance => new NullLoggerFactory();

        public ILoggingConfiguration Configuration { get; }

        private NullLoggerFactory()
        {
            Configuration = new NullLoggerConfiguration();
        }

        internal NullLoggerFactory(NullLoggerConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ILogger CreateLogger()
        {
            return NullLogger.Instance;
        }

        public ILogger CreateLogger(LoggerLevel loggerLevel)
        {
            return NullLogger.Instance;
        }
    }
}
