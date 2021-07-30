using System.IO;

namespace Neurocita.Reactive
{
    public interface IDeserializer
    {
        T Deserialize<T>(Stream stream);
    }
}
