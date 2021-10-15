using System.IO;

namespace Neurocita.Reactive
{
    public class XmlSerializer : ISerializer
    {
        public Stream Serialize<T>(T instance)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, instance);
            stream.Position = 0;
            return stream;
        }
    }
}
