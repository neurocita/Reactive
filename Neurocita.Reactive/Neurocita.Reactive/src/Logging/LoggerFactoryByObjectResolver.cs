using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public class LoggerFactoryByObjectResolver : ILoggerFactoryResolver
    {
        private readonly IDictionary<object, ILoggerFactory> register;

        public LoggerFactoryByObjectResolver()
            : this(new Dictionary<object, ILoggerFactory>())
        {
        }

        public LoggerFactoryByObjectResolver(IDictionary<object, ILoggerFactory> register)
        {
            this.register = register;
        }

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
