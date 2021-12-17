namespace Neurocita.Reactive.Pipeline
{
    public interface IPipelineTask<TPipelineContext> : IRuntimeTask<TPipelineContext>
        where TPipelineContext : IPipelineContext
    {
    }
}
