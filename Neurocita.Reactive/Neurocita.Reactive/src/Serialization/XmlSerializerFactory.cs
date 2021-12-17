namespace Neurocita.Reactive.Serialization
{
    public class XmlSerializerFactory : ISerializerFactory
    {
        public ISerializer Create()
        {
            return new XmlSerializer();
        }
    }
}
