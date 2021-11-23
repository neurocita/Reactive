using System;
using System.Collections.Generic;
using System.Text;

namespace Neurocita.Reactive
{
    public interface IPipelineSinkEndpoint : IDisposable
    {
        string Node { get; }
        IDisposable SubscribeTo<T>(IObservable<T> observable);
    }
}
