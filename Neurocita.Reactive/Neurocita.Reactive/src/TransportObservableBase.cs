using System;

namespace Neurocita.Reactive
{
    public abstract class TransportObservableBase : ITransportObservable
    {
        private readonly string address;

        protected TransportObservableBase(string address)
        {
            this.address = address;
        }

        public string Address => address;

        public abstract IDisposable Subscribe(IObserver<ITransportPipelineContext> observer);

        public void Dispose() => Dispose(true);

        protected abstract void Dispose(bool disposing);
    }
}
