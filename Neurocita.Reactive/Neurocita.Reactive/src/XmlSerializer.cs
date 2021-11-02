using System.IO;

namespace Neurocita.Reactive
{
    public class XmlSerializer : ISerializer
    {
        public string ContentType => "text/xml";

        public Stream Serialize<T>(T instance)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, instance);
            stream.Position = 0;
            return stream;
        }
        
        public T Deserialize<T>(Stream stream)
        {
            stream.Position = 0;
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            return (T) serializer.Deserialize(stream);
        }
    }
}
