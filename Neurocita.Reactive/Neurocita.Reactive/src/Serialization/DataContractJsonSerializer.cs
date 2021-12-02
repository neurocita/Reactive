using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Neurocita.Reactive
{
    public class DataContractJsonSerializer : ISerializer
    {
        private readonly DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();

        public DataContractJsonSerializer()
            : this(new HashSet<Type>())
        {
            
        }

        public DataContractJsonSerializer(IEnumerable<Type> knownTypes)
        {
            settings.KnownTypes = DataContractUtil.PrepareKnownTypes(knownTypes);
        }

        public DataContractJsonSerializer(DataContractJsonSerializerSettings settings)
        {
            this.settings = settings;
            this.settings.KnownTypes = DataContractUtil.PrepareKnownTypes(this.settings.KnownTypes);
        }

        public DataContractJsonSerializerSettings Settings => settings;
        public string ContentType => "application/json";

        public Stream Serialize<T>(T instance)
        {
            this.settings.KnownTypes = DataContractUtil.PrepareKnownTypes(this.settings.KnownTypes);
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
            this.settings.KnownTypes = DataContractUtil.PrepareKnownTypes(this.settings.KnownTypes);
            Type type = typeof(T);
            (settings.KnownTypes as ISet<Type>).Add(type);

            stream.Position = 0;
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(type, settings);
            return (T)serializer.ReadObject(stream);
        }
    }
}
