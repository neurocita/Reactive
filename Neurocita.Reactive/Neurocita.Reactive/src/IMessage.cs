using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public interface IMessage<T>
    {
        IDictionary<object, object> Headers { get; }
        T Body { get; }
    }
}
