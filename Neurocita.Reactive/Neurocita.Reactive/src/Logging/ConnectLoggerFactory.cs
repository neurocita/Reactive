using System;

namespace Neurocita.Reactive
{
    public class ConnectLoggerFactory : ILoggerFactory
    {
        private readonly ILogger logger;

        public ConnectLoggerFactory(ILogger logger)
        {
            this.logger = logger;
        }

        public ILogger CreateLogger(Type type)
        {
            return logger;
        }

        public ILogger CreateLogger(Type type, LoggerLevel loggerLevel)
        {
            return logger;
        }

        public ILogger CreateLogger<T>(T instance)
        {
            return logger;
        }

        public ILogger CreateLogger<T>(T instance, LoggerLevel loggerLevel)
        {
            return logger;
        }
    }
}
