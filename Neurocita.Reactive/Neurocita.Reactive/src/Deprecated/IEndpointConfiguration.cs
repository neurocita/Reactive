namespace Neurocita.Reactive
{
    public interface IEndpointConfiguration
    {
        IServiceConfiguration Service { get; }
        IEndpoint<T> Create<T>();
    }
}
