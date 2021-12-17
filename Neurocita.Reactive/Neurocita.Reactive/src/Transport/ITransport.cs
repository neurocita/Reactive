namespace Neurocita.Reactive.Transport
{
    public interface ITransport
    {
        ITransportMessageSource CreateSource(string source);
        ITransportMessageSink CreateSink(string destination);
        // ToDo: Create source / sink with filters
    }
}