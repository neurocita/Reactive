using System;

namespace Neurocita.Reactive
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
        ILogger CreateLogger(Type type, LoggerLevel loggerLevel);
        ILogger CreateLogger<T>(T instance);
        ILogger CreateLogger<T>(T instance, LoggerLevel loggerLevel);
    }
}
