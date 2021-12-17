using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Neurocita.Reactive.Serialization
{
    public class BinarySerializer : ISerializer
    {
        private readonly IFormatter formatter = new BinaryFormatter();

        public string ContentType => "application/octet-stream";

        public Stream Serialize<T>(T instance)
        {
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, instance);
            stream.Position = 0;
            return stream;
        }

        public T Deserialize<T>(Stream stream)
        {
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }
    }
}
