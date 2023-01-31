using System.Collections.ObjectModel;

namespace Neurocita.Reactive.Transport
{
    public class InMemoryTransportNodeCollection : KeyedCollection<string, IInMemoryTransportNode>
    {
        protected override string GetKeyForItem(IInMemoryTransportNode item)
        {
            return item.Path;
        }
    }
}
