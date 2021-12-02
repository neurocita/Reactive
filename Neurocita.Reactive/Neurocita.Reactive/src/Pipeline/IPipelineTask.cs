namespace Neurocita.Reactive
{
    public interface IPipelineTask<TPipelineContext> : IRuntimeTask<TPipelineContext>
        where TPipelineContext : IPipelineContext
    {
    }
}
