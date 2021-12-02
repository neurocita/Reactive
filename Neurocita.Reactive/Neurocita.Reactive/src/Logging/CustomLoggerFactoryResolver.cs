using System;

namespace Neurocita.Reactive
{
    public class CustomLoggerFactoryResolver : ILoggerFactoryResolver
    {
        private readonly Func<object, ILoggerFactory> resolver;

        public CustomLoggerFactoryResolver(Func<object, ILoggerFactory> resolver)
        {
            this.resolver = resolver;
        }

        public ILoggerFactory Resolve<T>(T instance)
        {
            return resolver.Invoke(instance);
        }
    }
}
