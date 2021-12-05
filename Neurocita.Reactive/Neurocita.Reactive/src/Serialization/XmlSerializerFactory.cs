namespace Neurocita.Reactive
{
    public class XmlSerializerFactory : ISerializerFactory
    {
        public ISerializer CreateSerializer()
        {
            return new XmlSerializer();
        }
    }
}
