namespace Neurocita.Reactive
{
    public interface IRuntimeTask<T> where T : IRuntimeContext
    {
        void Run(T context);
    }
}
