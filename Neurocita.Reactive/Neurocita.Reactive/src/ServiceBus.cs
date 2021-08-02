using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace Neurocita.Reactive
{
    public class ServiceBus : IServiceBus
    {
        private bool disposed = false;
        private readonly CompositeDisposable endpoints;

        public ServiceBus(IEnumerable<IDisposable> endpoints)
        {
            this.endpoints = new CompositeDisposable(endpoints);
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
                endpoints.Dispose();

            disposed = true;
        }
    }
}
