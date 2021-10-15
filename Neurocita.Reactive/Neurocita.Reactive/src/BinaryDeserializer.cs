using System.IO;
using System.Runtime.Serialization;

namespace Neurocita.Reactive
{
    public class BinaryDeserializer : IDeserializer
    {
        IFormatter formatter;

        internal BinaryDeserializer(IFormatter formatter)
        {
            this.formatter = formatter;
        }

        public T Deserialize<T>(Stream stream)
        {
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }
    }
}
