namespace Neurocita.Reactive
{
    public static class EndpointObservable
    {
        public static IEndpoint<T> Create<T>(IEndpointConfiguration<T> configuration)
        {
            return configuration.Create();
        }
    }
}
