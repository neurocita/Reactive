namespace Neurocita.Reactive
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger<T>(T instance);
        ILogger CreateLogger<T>(T instance, LoggerLevel loggerLevel);
    }
}
