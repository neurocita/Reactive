using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public class LoggerFactoryByTypeResolver : ILoggerFactoryResolver
    {
        private readonly IDictionary<Type, ILoggerFactory> register = new Dictionary<Type, ILoggerFactory>();

        public void Register(Type type, ILoggerFactory loggerFactory)
        {
            Util.CheckNullArgument(type);
            Util.CheckNullArgument(loggerFactory);
            register.Add(type, loggerFactory);
        }

        public bool Unregister(Type type)
        {
            Util.CheckNullArgument(type);
            return register.Remove(type);
        }

        public ILoggerFactory Resolve<T>(T instance)
        {
            Util.CheckNullArgument(instance);
            return register[instance.GetType()];
        }
    }
}
