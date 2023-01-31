using System.IO;

namespace Neurocita.Reactive.Serialization
{
    public interface ISerializer
    {
        string RfcContentType { get; }

        Stream Serialize<T>(T instance);
        T Deserialize<T>(Stream stream);
    }
}
