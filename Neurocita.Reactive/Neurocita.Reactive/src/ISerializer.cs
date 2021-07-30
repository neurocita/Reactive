using System.IO;

namespace Neurocita.Reactive
{
    public interface ISerializer
    {
        Stream Serialize<T>(T instance);
    }
}
