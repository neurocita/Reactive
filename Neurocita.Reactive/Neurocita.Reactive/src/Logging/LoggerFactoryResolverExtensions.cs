using System;

namespace Neurocita.Reactive
{
    public static class LoggerFactoryResolverExtensions
    {
        public static ILoggerFactory Resolve<T>(this ILoggerFactoryResolver loggerFactoryResolver)
        {
            return loggerFactoryResolver.Resolve(typeof(T));
        }

        public static ILoggerFactory Resolve<T>(this ILoggerFactoryResolver _, Func<Type, ILoggerFactory> resolver)
        {
            return resolver.Invoke(typeof(T));
        }

        public static ILoggerFactory Resolve<T>(this ILoggerFactoryResolver _, Func<Type, ILoggerFactory> resolver, Type type)
        {
            return resolver.Invoke(type);
        }

        public static ILoggerFactory Resolve<T>(this ILoggerFactoryResolver _, Func<T, ILoggerFactory> resolver, T instance)
        {
            return resolver.Invoke(instance);
        }
    }
}
