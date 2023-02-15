using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Neurocita.Reactive.Serialization
{
    internal class DataContractJsonSerializer : ISerializer
    {
        private readonly DataContractJsonSerializerSettings _settings;

        public DataContractJsonSerializer()
        {
            _settings = new DataContractJsonSerializerSettings();
        }

        public DataContractJsonSerializer(DataContractJsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public string ContentType => "application/json";

        public T Deserialize<T>(Stream stream)
        {
            Type type = typeof(T);
            (_settings.KnownTypes as ISet<Type>).Add(type);

            stream.Position = 0;
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(type, _settings);
            return (T)serializer.ReadObject(stream);
        }

        public Stream Serialize<T>(T instance)
        {
            Type type = instance.GetType();
            (_settings.KnownTypes as ISet<Type>).Add(type);

            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(type, _settings);
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, instance);
            stream.Position = 0;
            return stream;
        }
    }
}
