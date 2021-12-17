namespace Neurocita.Reactive.Pipeline
{
    public interface IPipelineObjectTask<TDataContract> : IPipelineTask<IPipelineObjectContext<TDataContract>>
        where TDataContract : IDataContract
    {
    }
}
