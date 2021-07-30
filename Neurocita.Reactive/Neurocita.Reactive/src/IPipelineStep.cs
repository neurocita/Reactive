namespace Neurocita.Reactive
{
    public interface IPipelineStep
    {
        IPipelineContext Invoke(IPipelineContext context);
    }
}
