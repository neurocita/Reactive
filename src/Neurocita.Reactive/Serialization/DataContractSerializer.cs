using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Neurocita.Reactive.Serialization
{
    public class DataContractSerializer : ISerializer
    {
        public static DataContractSerializer Xml => new DataContractSerializer(DataContractSerializeFormat.Xml);
        public static DataContractSerializer Json => new DataContractSerializer(DataContractSerializeFormat.Json);
    
        private readonly XmlObjectSerializerFactory _serializerFactory;

        public DataContractSerializer(DataContractSerializeFormat format)
        {
            _serializerFactory = new XmlObjectSerializerFactory(format);
            Format = format;
            switch (format)
            {
                case DataContractSerializeFormat.Xml:
                    ContentType = DataContractContentType.TextXml;
                    break;

                case DataContractSerializeFormat.Json:
                    ContentType = DataContractContentType.ApplicationJson;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(format));
            }
        }

        public DataContractSerializer(DataContractSerializerSettings settings)
        {
            _serializerFactory = new XmlObjectSerializerFactory(settings);
            ContentType = DataContractContentType.TextXml;
            Format = DataContractSerializeFormat.Xml;
        }

        public DataContractSerializer(DataContractJsonSerializerSettings settings)
        {
            _serializerFactory = new XmlObjectSerializerFactory(settings);
            ContentType = DataContractContentType.ApplicationJson;
            Format = DataContractSerializeFormat.Json;
        }

        public string ContentType { get; }
        public DataContractSerializeFormat Format { get; }

        public T Deserialize<T>(Stream stream)
        {
            stream.Position = 0;
            XmlObjectSerializer serializer = _serializerFactory.Create(typeof(T));
            return (T)serializer.ReadObject(stream);
        }

        public Stream Serialize<T>(T instance)
        {
            XmlObjectSerializer serializer = _serializerFactory.Create(typeof(T));
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, instance);
            stream.Position = 0;
            return stream;
        }
    }
}
