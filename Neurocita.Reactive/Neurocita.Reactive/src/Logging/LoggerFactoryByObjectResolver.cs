using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public class LoggerFactoryByObjectResolver : ILoggerFactoryResolver
    {
        private readonly IDictionary<object, ILoggerFactory> register = new Dictionary<object, ILoggerFactory>();

        public void Register(object instance, ILoggerFactory loggerFactory)
        {
            register.Add(instance, loggerFactory);
        }

        public bool Unregister(object instance)
        {
            return register.Remove(instance);
        }

        public ILoggerFactory Resolve<T>(T instance)
        {
            return register[instance];
        }
    }
}
