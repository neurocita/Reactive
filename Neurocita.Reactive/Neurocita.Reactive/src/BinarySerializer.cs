using System.IO;
using System.Runtime.Serialization;

namespace Neurocita.Reactive
{
    public class BinarySerializer : ISerializer
    {
        IFormatter formatter;

        internal BinarySerializer(IFormatter formatter)
        {
            this.formatter = formatter;
        }

        public Stream Serialize<T>(T instance)
        {
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, instance);
            stream.Position = 0;
            return stream;
        }
    }
}
