using System;
using System.Collections.Generic;
using System.Text;

namespace Neurocita.Reactive.Logging
{
    public interface ILogContextProperty
    {
        void Add(string key, object value);
    }
}
