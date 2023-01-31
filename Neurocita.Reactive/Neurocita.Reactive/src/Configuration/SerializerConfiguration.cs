using Neurocita.Reactive.Serialization;

namespace Neurocita.Reactive.Configuration
{
    public delegate ISerializer SerializerFactory(params object[] input);

    public class SerializerConfiguration
    {
        internal SerializerFactory Factory { get; private set; }
    }
}
