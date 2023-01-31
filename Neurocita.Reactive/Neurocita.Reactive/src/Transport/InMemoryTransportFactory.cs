namespace Neurocita.Reactive.Transport
{
    public class InMemoryTransportFactory : ITransportFactory<InMemoryTransport>
    {
        public InMemoryTransport Create()
        {
            return new InMemoryTransport();
        }
    }
}
