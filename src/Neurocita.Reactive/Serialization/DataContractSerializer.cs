using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Neurocita.Reactive.Serialization
{
    public class DataContractSerializer : ISerializer
    {
        private readonly IDataContractInnerSerializerFactory _innerSerializerFactory;
        private readonly string _contentType;

        public DataContractSerializer(DataContractSerializerSettings settings)
        {
            _innerSerializerFactory = new DataContractInnerXmlSerializerFactory(settings);
            _contentType = "text/xml";
        }

        public DataContractSerializer(DataContractJsonSerializerSettings settings)
        {
            _innerSerializerFactory = new DataContractInnerJsonSerializerFactory(settings);
            _contentType = "application/json";
        }

        public string ContentType => _contentType;

        public static DataContractSerializer Json() => new DataContractSerializer(new DataContractJsonSerializerSettings());
        public static DataContractSerializer Xml() => new DataContractSerializer(new DataContractSerializerSettings());

        public T Deserialize<T>(Stream stream)
        {
            stream.Position = 0;
            XmlObjectSerializer serializer = _innerSerializerFactory.Create(typeof(T));
            return (T)serializer.ReadObject(stream);
        }

        public Stream Serialize<T>(T instance)
        {
            XmlObjectSerializer serializer = _innerSerializerFactory.Create(typeof(T));
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, instance);
            stream.Position = 0;
            return stream;
        }
    }
}
