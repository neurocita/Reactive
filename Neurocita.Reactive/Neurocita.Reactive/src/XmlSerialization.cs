namespace Neurocita.Reactive
{
    public class XmlSerialization : ISerialization
    {
        public string ContentType => "text/xml";

        public IDeserializer CreateDeserializer()
        {
            return new XmlDeserializer();
        }

        public ISerializer CreateSerializer()
        {
            return new XmlSerializer();
        }
    }
}
