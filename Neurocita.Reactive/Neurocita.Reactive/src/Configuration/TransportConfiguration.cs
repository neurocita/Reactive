using System;
using Neurocita.Reactive;
using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive.Configuration
{
    public delegate ITransport TransportFactory(params object[] input);
    
    public class TransportConfiguration
    {
        private readonly IServiceBusBuilder _serviceBusBuilder;
        private TransportFactory _transportFactory;

        internal TransportConfiguration(IServiceBusBuilder serviceBusBuilder)
        {
            _serviceBusBuilder = serviceBusBuilder;
        }

        public IServiceBusBuilder Using<TTransportFactory>(params object[] input)
            where TTransportFactory : Delegate, new()
        {
            if (typeof(TTransportFactory).IsSubclassOf(typeof(TransportFactory)))
                throw new NotSupportedException(); ;

            _transportFactory = new TTransportFactory() as TransportFactory;
            return _serviceBusBuilder;
        }

        internal TransportFactory Factory { get => _transportFactory; }
    }
}
