using System;

namespace Neurocita.Reactive
{
    public interface ITransport : IObservable<ITransportPipelineContext>, IObserver<ITransportPipelineContext>, IDisposable
    {

    }
}
