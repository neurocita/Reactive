using System;

namespace Neurocita.Reactive
{
    public static class ObservableExtensions
    {
        public static IEndpoint<T> ToEndpoint<T>(this IObservable<T> observable, IEndpointConfiguration<T> configuration)
        {
            return configuration.Create(observable);
        }
    }
}
