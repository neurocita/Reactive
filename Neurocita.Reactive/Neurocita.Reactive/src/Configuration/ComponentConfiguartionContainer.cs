namespace Neurocita.Reactive.Configuration
{
    internal class ComponentConfiguartionContainer<TComponent>
    {
        private readonly IServiceBusConfiguration _busConfiguration;
        private readonly TComponent _component;

        internal ComponentConfiguartionContainer(IServiceBusConfiguration serviceBusConfiguration, TComponent component)
        {
            _busConfiguration = serviceBusConfiguration;
            _component = component;
        }

        public TComponent Component => _component;
        public IServiceBusConfiguration BusConfiguration => _busConfiguration;
    }
}
