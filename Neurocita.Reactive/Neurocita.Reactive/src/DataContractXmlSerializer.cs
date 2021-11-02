using System.IO;
using System.Runtime.Serialization;

namespace Neurocita.Reactive
{
    public class DataContractXmlSerializer : ISerializer
    {
        private readonly DataContractSerializerSettings settings = new DataContractSerializerSettings();

        public DataContractSerializerSettings Settings => settings;
        public string ContentType => "text/xml";

        public Stream Serialize<T>(T instance)
        {
            MemoryStream stream = new MemoryStream();
            System.Runtime.Serialization.DataContractSerializer serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T), settings);
            serializer.WriteObject(stream, instance);
            stream.Position = 0;
            return stream;
        }

        public T Deserialize<T>(Stream stream)
        {
            stream.Position = 0;
            System.Runtime.Serialization.DataContractSerializer serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T), settings);
            return (T)serializer.ReadObject(stream);
        }
    }
}
