using System;
using System.Collections.Generic;
using System.Text;

namespace Neurocita.Reactive
{
    public interface IObervableServiceBus<T> : IObservable<T>, IServiceBus
    {
    }
}
