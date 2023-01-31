using Neurocita.Reactive.Configuration;

namespace Neurocita.Reactive.Serialization
{
    public interface ISerializerFactory
    {
        SerializerConfiguration Configuration { get; }
        ISerializer Create();
    }
}
