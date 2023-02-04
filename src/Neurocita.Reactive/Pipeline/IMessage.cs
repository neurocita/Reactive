using System;
using System.Collections.Generic;

namespace Neurocita.Reactive.Pipeline
{
    public interface IMessage<T>
    {
        IDictionary<string,object> Headers { get; }
        T Body { get; }
    }
}