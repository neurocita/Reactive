namespace Neurocita.Reactive.Serialization
{
    public class BinarySerializerFactory : ISerializerFactory
    {
        public ISerializer Create()
        {
            return new BinarySerializer();
        }
    }
}
