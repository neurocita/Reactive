namespace Neurocita.Reactive
{
    public interface IPipelineObjectContext<TDataContract> : IPipelineContext
        where TDataContract : IDataContract
    {
        IMessage<TDataContract> Message { get; }
    }
}
