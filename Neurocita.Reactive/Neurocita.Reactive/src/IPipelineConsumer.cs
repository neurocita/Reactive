namespace Neurocita.Reactive
{
    public interface IPipelineConsumer
    {
        void Invoke(IPipelineContext pipelineContext);
    }
}
