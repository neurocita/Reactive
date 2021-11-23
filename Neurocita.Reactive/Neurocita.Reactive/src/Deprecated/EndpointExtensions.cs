using System;

namespace Neurocita.Reactive
{
    public static class EndpointExtensions
    {
        public static IEndpoint<T> Create<T>(this IEndpointConfiguration configuration, IObservable<T> producer)
        {
            IEndpoint<T> endpoint = configuration.Create<T>();
            producer.Subscribe(endpoint);
            return endpoint;
        }
    }
}
