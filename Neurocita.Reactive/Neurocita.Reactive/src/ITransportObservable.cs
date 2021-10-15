using System;

namespace Neurocita.Reactive
{
    public interface ITransportObservable : IDisposableObservable<ITransportPipelineContext>
    {
    }
}
