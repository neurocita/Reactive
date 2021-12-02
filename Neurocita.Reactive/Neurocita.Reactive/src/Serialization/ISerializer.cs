using System.IO;

namespace Neurocita.Reactive
{
    public interface ISerializer
    {
        string ContentType { get; }

        Stream Serialize<T>(T instance);
        T Deserialize<T>(Stream stream);
    }
}
