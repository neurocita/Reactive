namespace Neurocita.Reactive
{
    public interface IPipelineTask<T> : IRuntimeTask<T> where T : IPipelineContext
    {
    }
}
