using Neurocita.Reactive.Transport;
using NATS.Client;

namespace Neurocita.Reactive.Configuration
{
    public static class NatsCanAddTransportExtensions
    {
        public static ICanAddSerializer TransportWithNats(this ICanAddTransport canAddTransport)
        {
            return canAddTransport.WithTransport(new NatsTransportFactory());
        }

        public static INatsCanAddSecureOrUserCredentials TransportWithNats(this ICanAddTransport canAddTransport, string url)
        {
            return new NatsTransportConfiguration(canAddTransport).WithUrl(url);
        }

        public static ICanAddSerializer TransportWithNats(this ICanAddTransport canAddTransport, Options options)
        {
            return canAddTransport.WithTransport(new NatsTransportFactory(options));
        }
    }
}