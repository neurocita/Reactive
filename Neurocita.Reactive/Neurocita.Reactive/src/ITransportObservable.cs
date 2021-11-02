namespace Neurocita.Reactive
{
    public interface ITransportObservable : IDisposableObservable<ITransportPipelineContext>
    {
        string Address { get; }
    }
}
