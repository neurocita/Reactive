using System.IO;

namespace Neurocita.Reactive
{
    public class DataContractJsonSerializer : ISerializer
    {
        private readonly DataContractJsonSerialization serialization;

        internal DataContractJsonSerializer(DataContractJsonSerialization serialization)
        {
            this.serialization = serialization;
        }
        public Stream Serialize<T>(T instance)
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T), serialization.Settings);
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, instance);
            stream.Position = 0;
            return stream;
        }
    }
}
