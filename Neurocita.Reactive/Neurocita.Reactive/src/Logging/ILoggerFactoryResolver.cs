using System;

namespace Neurocita.Reactive
{
    public interface ILoggerFactoryResolver
    {
        ILoggerFactory Resolve(Type type);
        ILoggerFactory Resolve<T>(T instance);
    }
}
