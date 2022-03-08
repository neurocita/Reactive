namespace Neurocita.Reactive.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger();
        ILogger CreateLogger(LoggerLevel loggerLevel);
    }
}
