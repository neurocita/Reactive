namespace Neurocita.Reactive
{
    public interface ISerializable
    {
        ISerializer Serializer { get; }
        IDeserializer Deserializer { get; }
    }
}
