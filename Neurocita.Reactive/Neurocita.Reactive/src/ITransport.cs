using System;

namespace Neurocita.Reactive
{
    public interface ITransport : IObservable<IPipelineContext>, IObserver<IPipelineContext>, IDisposable
    {

    }
}
