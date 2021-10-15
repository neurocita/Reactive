using System.IO;

namespace Neurocita.Reactive
{
    public class DataContractJsonDeserializer : IDeserializer
    {
        private readonly DataContractJsonSerialization serialization;

        internal DataContractJsonDeserializer(DataContractJsonSerialization serialization)
        {
            this.serialization = serialization;
        }

        public T Deserialize<T>(Stream stream)
        {
            stream.Position = 0;
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T), serialization.Settings);
            return (T) serializer.ReadObject(stream);
        }
    }
}
