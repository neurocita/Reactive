using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public class LoggerFactoryByTypeResolver : ILoggerFactoryResolver
    {
        private readonly IDictionary<Type, ILoggerFactory> register;

        public LoggerFactoryByTypeResolver()
            : this(new Dictionary<Type, ILoggerFactory>())
        {
        }

        public LoggerFactoryByTypeResolver(IDictionary<Type, ILoggerFactory> register)
        {
            this.register = register;
        }

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

        public ILoggerFactory Resolve(Type type)
        {
            return register[type];
        }

        public ILoggerFactory Resolve<T>(T instance)
        {
            return Resolve(instance.GetType());
        }
    }
}
