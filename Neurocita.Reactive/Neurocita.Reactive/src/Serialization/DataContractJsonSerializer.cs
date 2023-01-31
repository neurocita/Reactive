using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Neurocita.Reactive.Serialization
{
    public class DataContractJsonSerializer : ISerializer
    {
        private readonly DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();

        internal DataContractJsonSerializer(DataContractJsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public string RfcContentType => "application/json";

        public Stream Serialize<T>(T instance)
        {
            Type type = instance.GetType();
            (settings.KnownTypes as ISet<Type>).Add(type);

            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(type, settings);
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, instance);
            stream.Position = 0;
            return stream;
        }

        public T Deserialize<T>(Stream stream)
        {
            Type type = typeof(T);
            (settings.KnownTypes as ISet<Type>).Add(type);

            stream.Position = 0;
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(type, settings);
            return (T)serializer.ReadObject(stream);
        }
    }
}
