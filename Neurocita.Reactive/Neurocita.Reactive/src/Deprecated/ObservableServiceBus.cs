using System;
using System.Collections.Generic;
using System.Text;

namespace Neurocita.Reactive
{
    public class ObservableServiceBus<T> : IObervableServiceBus<T>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }
}
