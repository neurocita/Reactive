namespace Neurocita.Reactive
{
    public interface ILoggerFactoryResolver
    {
        ILoggerFactory Resolve<T>(T instance);
    }
}
