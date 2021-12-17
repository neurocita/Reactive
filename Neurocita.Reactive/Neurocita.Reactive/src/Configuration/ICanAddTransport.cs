using Neurocita.Reactive.Transport;

namespace Neurocita.Reactive.Configuration
{
    public interface ICanAddTransport
    {
        ICanAddSerializer WithTransport(ITransportFactory transportFactory);
    }
}