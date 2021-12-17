using System.Runtime.Serialization.Json;

namespace Neurocita.Reactive.Serialization
{
    public class DataContractJsonSerializerFactory : ISerializerFactory
    {
        private readonly DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();

        public DataContractJsonSerializerSettings Settings => settings;

        public ISerializer Create()
        {
            settings.KnownTypes = DataContractUtil.PrepareKnownTypes(settings.KnownTypes);
            return new DataContractJsonSerializer(settings);
        }
    }
}
