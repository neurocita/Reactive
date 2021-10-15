using System;

namespace Neurocita.Reactive
{
    public interface ITransportObserver : IObserver<ITransportPipelineContext>, IDisposable
    {
    }
}
