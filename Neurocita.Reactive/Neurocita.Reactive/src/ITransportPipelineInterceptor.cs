namespace Neurocita.Reactive
{
    public interface ITransportPipelineInterceptor
    {
        ITransportPipelineContext Invoke(ITransportPipelineContext context);
    }
}
