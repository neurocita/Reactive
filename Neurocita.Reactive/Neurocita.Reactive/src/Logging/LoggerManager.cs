using System;

namespace Neurocita.Reactive.Logging
{
    public sealed class LoggerManager
    {
        private static Lazy<ILoggerFactory> loggerFactory;
        
        private LoggerManager(Func<ILoggerFactory> creator)
        {
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));
            
            loggerFactory = new Lazy<ILoggerFactory> (() => NullLoggerFactory.Instance);
        }

        public static ILogger GetLogger()
        {
            return loggerFactory.Value.CreateLogger();
        }
    }
}