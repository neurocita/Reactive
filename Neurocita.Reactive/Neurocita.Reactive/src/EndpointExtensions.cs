using System;

namespace Neurocita.Reactive
{
    public static class EndpointExtensions
    {
        public static IEndpoint<T> Create<T>(this IEndpointConfiguration<T> configuration, IObservable<T> producer)
        {
            IEndpoint<T> endpoint = configuration.Create();
            producer.Subscribe(endpoint);
            return endpoint;
        }
    }
}
