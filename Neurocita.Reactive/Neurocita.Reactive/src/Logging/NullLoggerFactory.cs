using System;

namespace Neurocita.Reactive.Logging
{
    public sealed class NullLoggerFactory : ILoggerFactory
    {
        public static ILoggerFactory Instance => new NullLoggerFactory();

        private NullLoggerFactory()
        { }

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
