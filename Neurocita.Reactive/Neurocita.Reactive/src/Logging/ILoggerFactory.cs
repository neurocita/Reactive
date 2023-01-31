using Neurocita.Reactive.Configuration;

namespace Neurocita.Reactive.Logging
{
    public interface ILoggerFactory
    {
        ILoggingConfiguration Configuration { get; }
        ILogger CreateLogger();
        ILogger CreateLogger(LoggerLevel loggerLevel);
    }
}
