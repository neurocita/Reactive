using Neurocita.Reactive.Serialization;

namespace Neurocita.Reactive.Configuration
{
    public interface ICanAddSerializer
    {
        ICanAddEndpoint WithSerializer(ISerializerFactory serializerFactory);
    }
}