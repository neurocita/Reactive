using System;

namespace Neurocita.Reactive
{
    public abstract class TransportObserverBase : ITransportObserver
    {
        private readonly IRuntimeContext runtimeContext;
        private readonly string address;

        protected TransportObserverBase(IRuntimeContext runtimeContext, string address)
        {
            this.runtimeContext = runtimeContext;
            this.address = address;
        }

        public string Address => address;

        public void OnCompleted()
        {
            Dispose(true);
        }

        public void OnError(Exception error)
        {
            runtimeContext.Log.Error(error);
            Dispose(true);
        }

        public abstract void OnNext(ITransportPipelineContext value);

        public void Dispose() => Dispose(true);

        protected abstract void Dispose(bool disposing);
    }
}
