using System.Collections.ObjectModel;

namespace Neurocita.Reactive
{
    internal class EndpointCollection : KeyedCollection<string, IEndpoint>
    {
        protected override string GetKeyForItem(IEndpoint item)
        {
            return item.NodePath;
        }
    }
}
