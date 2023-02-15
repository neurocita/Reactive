using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Neurocita.Reactive.Serialization
{
    internal class DataContractXmlSerializer : ISerializer
    {
        private readonly DataContractSerializerSettings _settings;

        public DataContractXmlSerializer()
        {
            _settings = new DataContractSerializerSettings();
        }

        public DataContractXmlSerializer(DataContractSerializerSettings settings)
        {
            _settings = settings;
        }

        public string ContentType => "text/xml";

        public T Deserialize<T>(Stream stream)
        {
            Type type = typeof(T);
            (_settings.KnownTypes as ISet<Type>).Add(type);

            stream.Position = 0;
            System.Runtime.Serialization.DataContractSerializer serializer = new System.Runtime.Serialization.DataContractSerializer(type, _settings);
            return (T)serializer.ReadObject(stream);
        }

        public Stream Serialize<T>(T instance)
        {
            Type type = instance.GetType();
            (_settings.KnownTypes as ISet<Type>).Add(type);

            System.Runtime.Serialization.DataContractSerializer serializer = new System.Runtime.Serialization.DataContractSerializer(type, _settings);
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, instance);
            stream.Position = 0;
            return stream;
        }
    }
}
