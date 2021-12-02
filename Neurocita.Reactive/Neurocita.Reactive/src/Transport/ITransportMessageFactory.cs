namespace Neurocita.Reactive
{
    public interface ITransportMessageFactory
    {
        ITransportMessageSource CreateSource(string source);
        ITransportMessageSink CreateSink(string destination);
        // ToDo: Create source / sink with filters
    }
}
