using System.IO;

namespace Neurocita.Reactive
{
    public class DataContractXmlSerializer : ISerializer
    {
        private readonly DataContractXmlSerialization serialization;

        internal DataContractXmlSerializer(DataContractXmlSerialization serialization)
        {
            this.serialization = serialization;
        }

        public Stream Serialize<T>(T instance)
        {
            MemoryStream stream = new MemoryStream();
            System.Runtime.Serialization.DataContractSerializer serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T), serialization.Settings);
            serializer.WriteObject(stream, instance);
            stream.Position = 0;
            return stream;
        }
    }
}
