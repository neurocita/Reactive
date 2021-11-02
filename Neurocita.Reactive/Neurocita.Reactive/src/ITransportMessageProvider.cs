namespace Neurocita.Reactive
{
    public interface ITransportMessageProvider
    {
        ITransportMessageSource CreateSource(string address);
        ITransportMessageSink CreateSink(string address);
    }
}
