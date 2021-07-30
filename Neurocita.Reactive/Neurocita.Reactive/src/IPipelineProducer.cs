namespace Neurocita.Reactive
{
    public interface IPipelineProducer
    {
        IPipelineContext Invoke();
    }
}
