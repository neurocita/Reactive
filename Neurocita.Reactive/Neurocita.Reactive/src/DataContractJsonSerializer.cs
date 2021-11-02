using System.IO;
using System.Runtime.Serialization.Json;

namespace Neurocita.Reactive
{
    public class DataContractJsonSerializer : ISerializer
    {
        private readonly DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();

        public DataContractJsonSerializerSettings Settings => settings;
        public string ContentType => "application/json";

        public Stream Serialize<T>(T instance)
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T), settings);
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, instance);
            stream.Position = 0;
            return stream;
        }

        public T Deserialize<T>(Stream stream)
        {
            stream.Position = 0;
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T), settings);
            return (T)serializer.ReadObject(stream);
        }
    }
}
