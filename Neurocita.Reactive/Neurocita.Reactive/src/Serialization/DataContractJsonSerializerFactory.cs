using System.Runtime.Serialization.Json;

namespace Neurocita.Reactive
{
    public class DataContractJsonSerializerFactory : ISerializerFactory
    {
        private readonly DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();

        public DataContractJsonSerializerSettings Settings => settings;

        public ISerializer CreateSerializer()
        {
            settings.KnownTypes = DataContractUtil.PrepareKnownTypes(settings.KnownTypes);
            return new DataContractJsonSerializer(settings);
        }
    }
}
