using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Neurocita.Reactive
{
    public class BinarySerialization : ISerialization
    {
        IFormatter formatter = new BinaryFormatter();

        public string ContentType => "application/octet-stream";

        public IDeserializer CreateDeserializer()
        {
            return new BinaryDeserializer(formatter);
        }

        public ISerializer CreateSerializer()
        {
            return new BinarySerializer(formatter);
        }
    }
}
