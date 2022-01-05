using Neurocita.Reactive.Serialization;

namespace Neurocita.Reactive.Configuration
{
    public static class CanAddSerializerExtensions
    {
        public static ICanAddEndpoint SerializeWithBinary(this ICanAddSerializer canAddSerializer)
        {
            return canAddSerializer.WithSerializer(new BinarySerializerFactory());
        }

        public static ICanAddEndpoint SerializeWithDataContractJson(this ICanAddSerializer canAddSerializer)
        {
            return canAddSerializer.WithSerializer(new DataContractJsonSerializerFactory());
        }

        public static ICanAddEndpoint SerializeWithDataContractXml(this ICanAddSerializer canAddSerializer)
        {
            return canAddSerializer.WithSerializer(new DataContractXmlSerializerFactory());
        }

        public static ICanAddEndpoint SerializeWithXml(this ICanAddSerializer canAddSerializer)
        {
            return canAddSerializer.WithSerializer(new XmlSerializerFactory());
        }
    }
}