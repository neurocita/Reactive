namespace Neurocita.Reactive
{
    public interface ITransportMessageFactory
    {
        ITransportMessageSource CreateSource(string node);
        ITransportMessageSink CreateSink(string node);
        // ToDo: Create source / sink with filters
    }
}
