namespace Neurocita.Reactive
{
    public class ConnectLoggerFactoryResolver : ILoggerFactoryResolver
    {
        private readonly ILoggerFactory loggerFactory;

        public ConnectLoggerFactoryResolver(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        public ILoggerFactoryResolver From(ILoggerFactory loggerFactory)
        {
            return new ConnectLoggerFactoryResolver(loggerFactory);
        }

        public ILoggerFactory Resolve<T>(T instance)
        {
            return loggerFactory;
        }
    }
}
