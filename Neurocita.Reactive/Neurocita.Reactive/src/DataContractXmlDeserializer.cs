using System.IO;

namespace Neurocita.Reactive
{
    public class DataContractXmlDeserializer : IDeserializer
    {
        private readonly DataContractXmlSerialization serialization;

        internal DataContractXmlDeserializer(DataContractXmlSerialization serialization)
        {
            this.serialization = serialization;
        }

        public T Deserialize<T>(Stream stream)
        {
            stream.Position = 0;
            System.Runtime.Serialization.DataContractSerializer serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T), serialization.Settings);
            return (T)serializer.ReadObject(stream);
        }
    }
}
