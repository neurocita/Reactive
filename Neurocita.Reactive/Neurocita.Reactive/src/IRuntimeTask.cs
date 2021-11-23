namespace Neurocita.Reactive
{
    public interface IRuntimeTask<TRuntimeContext>
        where TRuntimeContext : IRuntimeContext
    {
        void Run(TRuntimeContext context);
    }
}
