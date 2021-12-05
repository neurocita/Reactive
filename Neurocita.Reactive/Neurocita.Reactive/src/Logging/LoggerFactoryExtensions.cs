using System;

namespace Neurocita.Reactive
{
    public static class LoggerFactoryExtensions
    {
        public static ILogger CreateLogger<T>(this ILoggerFactory loggerFactory)
        {
            return loggerFactory.CreateLogger(typeof(T));
        }

        public static ILogger CreateLogger<T>(this ILoggerFactory loggerFactory, LoggerLevel loggerLevel)
        {
            return loggerFactory.CreateLogger(typeof(T), loggerLevel);
        }

        public static ILogger CreateLogger(this ILoggerFactory loggerFactory, string typeFullName)
        {
            return loggerFactory.CreateLogger(Type.GetType(typeFullName));
        }

        public static ILogger CreateLogger(this ILoggerFactory loggerFactory, string typeFullName, LoggerLevel loggerLevel)
        {
            return loggerFactory.CreateLogger(Type.GetType(typeFullName), loggerLevel);
        }

        public static ILogger CreateLogger<T>(this ILoggerFactory _, Func<Type, ILogger> creator)
        {
            return creator.Invoke(typeof(T));
        }

        public static ILogger CreateLogger(this ILoggerFactory _, Func<Type, ILogger> creator, Type type)
        {
            return creator.Invoke(type);
        }

        public static ILogger CreateLogger<T>(this ILoggerFactory _, Func<Type, LoggerLevel, ILogger> creator, LoggerLevel loggerLevel)
        {
            return creator.Invoke(typeof(T), loggerLevel);
        }

        public static ILogger CreateLogger(this ILoggerFactory _, Func<Type, LoggerLevel, ILogger> creator, Type type, LoggerLevel loggerLevel)
        {
            return creator.Invoke(type, loggerLevel);
        }

        public static ILogger CreateLogger<T>(this ILoggerFactory _, Func<T, ILogger> creator, T instance)
        {
            return creator.Invoke(instance);
        }

        public static ILogger CreateLogger<T>(this ILoggerFactory _, Func<T, LoggerLevel, ILogger> creator, T instance, LoggerLevel loggerLevel)
        {
            return creator.Invoke(instance, loggerLevel);
        }
    }
}
