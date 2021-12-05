namespace Neurocita.Reactive
{
    public class BinarySerializerFactory : ISerializerFactory
    {
        public ISerializer CreateSerializer()
        {
            return new BinarySerializer();
        }
    }
}
