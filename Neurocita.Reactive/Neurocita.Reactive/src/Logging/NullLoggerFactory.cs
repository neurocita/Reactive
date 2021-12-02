namespace Neurocita.Reactive
{
    public class NullLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger<T>(T instance)
        {
            return NullLogger.Instance;
        }

        public ILogger CreateLogger<T>(T instance, LoggerLevel loggerLevel)
        {
            return NullLogger.Instance;
        }
    }
}
