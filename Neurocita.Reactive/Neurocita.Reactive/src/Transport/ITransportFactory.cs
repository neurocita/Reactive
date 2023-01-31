namespace Neurocita.Reactive.Transport
{
    public interface ITransportFactory<T>
        where T : ITransport
    {
       T Create();
    }
}