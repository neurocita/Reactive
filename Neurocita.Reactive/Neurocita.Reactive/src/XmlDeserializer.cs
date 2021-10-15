using System.IO;

namespace Neurocita.Reactive
{
    public class XmlDeserializer : IDeserializer
    {
        public T Deserialize<T>(Stream stream)
        {
            stream.Position = 0;
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            return (T) serializer.Deserialize(stream);
        }
    }
}
