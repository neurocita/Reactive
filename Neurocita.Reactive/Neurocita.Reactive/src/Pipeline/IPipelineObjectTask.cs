namespace Neurocita.Reactive
{
    public interface IPipelineObjectTask<TDataContract> : IPipelineTask<IPipelineObjectContext<TDataContract>>
        where TDataContract : IDataContract
    {
    }
}
