using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;

namespace Neurocita.Reactive
{
    public class ServiceBus : IServiceBus
    {
        private bool disposed = false;
        private readonly object mutex = new object();
        private readonly IReadOnlyDictionary<string, IEndpoint> endpoints;

        public IReadOnlyDictionary<string, IEndpoint> Endpoints => endpoints;

        public ServiceBus(IDictionary<string, IEndpoint> endpoints)
        {
            this.endpoints = new ReadOnlyDictionary<string, IEndpoint>(endpoints);
        }
        
        public static IServiceBusConfiguration Configure()
        {
            return new ServiceBusConfiguration();
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                lock (mutex)
                {
                    new CompositeDisposable(endpoints.Values).Dispose();
                    disposed = true;
                }

        }
    }
}
