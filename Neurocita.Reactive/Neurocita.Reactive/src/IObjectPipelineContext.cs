namespace Neurocita.Reactive
{
    public interface IObjectPipelineContext : IPipelineContext
    {
        IMessage<object> Message { get; }
    }
}
