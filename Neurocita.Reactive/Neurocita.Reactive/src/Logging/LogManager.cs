using System;

namespace Neurocita.Reactive
{
    public class LogManager
    {
        private static ILoggerFactory _fallback = Default;
        private static ILoggerFactoryResolver _resolver;

        public static readonly ILoggerFactory Default = new NullLoggerFactory();

        public static void SetFallback(ILoggerFactory fallback)
        {
            Util.CheckNullArgument(fallback);
            _fallback = fallback;
        }
        public static void SetResolver (ILoggerFactoryResolver resolver)
        {
            Util.CheckNullArgument(resolver);
            _resolver = resolver;
        }

        public static ILoggerFactory GetFactory<T>(T instance)
        {
            return _resolver?.Resolve(instance) ?? _fallback;
        }

        public static ILoggerFactory GetFactory<T>(T instance, Func<T, ILoggerFactory> resolver)
        {
            return resolver.Invoke(instance) ?? _fallback;
        }

        public static void Reset()
        {
            _fallback = Default;
            _resolver = null;
        }
    }
}