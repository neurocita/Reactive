using System.Runtime.Serialization.Json;

namespace Neurocita.Reactive
{
    public class DataContractJsonSerialization : ISerialization
    {
        private readonly DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();

        public DataContractJsonSerializerSettings Settings => settings;
        public string ContentType => "application/json";

        public IDeserializer CreateDeserializer()
        {
            return new DataContractJsonDeserializer(this);
        }

        public ISerializer CreateSerializer()
        {
            return new DataContractJsonSerializer(this);
        }
    }
}
