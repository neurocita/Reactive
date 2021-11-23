namespace Neurocita.Reactive
{
    public static class EndpointObservable
    {
        public static IEndpoint<T> FromEndpoint<T>(IEndpointConfiguration configuration)
        {
            return configuration.Create<T>();
        }
    }
}
