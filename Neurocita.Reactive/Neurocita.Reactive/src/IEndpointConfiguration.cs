namespace Neurocita.Reactive
{
    public interface IEndpointConfiguration<T>
    {
        IServiceConfiguration Service { get; }
        IEndpoint<T> Create();
    }
}
