using System.Runtime.Serialization;

namespace Neurocita.Reactive
{
    public class DataContractXmlSerialization : ISerialization
    {
        private readonly DataContractSerializerSettings settings = new DataContractSerializerSettings();

        public DataContractSerializerSettings Settings => settings;
        public string ContentType => "text/xml";

        IDeserializer ISerialization.CreateDeserializer()
        {
            return new DataContractXmlDeserializer(this);
        }

        ISerializer ISerialization.CreateSerializer()
        {
            return new DataContractXmlSerializer(this);
        }
    }
}
