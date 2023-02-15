using System;
using System.IO;

namespace Neurocita.Reactive.Serialization
{
    public class DataContractSerializer : ISerializer
    {
        private ISerializer _innerSerializer;

        private DataContractSerializer(ISerializer innerSerializer)
        {
            _innerSerializer = innerSerializer;
        }

        public static DataContractSerializer Json() => new DataContractSerializer(new DataContractJsonSerializer());
        public static DataContractSerializer Xml() => new DataContractSerializer(new DataContractXmlSerializer());

        public string ContentType => _innerSerializer.ContentType;

        public T Deserialize<T>(Stream stream) => _innerSerializer.Deserialize<T>(stream);
        public Stream Serialize<T>(T instance) => _innerSerializer.Serialize(instance);
    }
}
