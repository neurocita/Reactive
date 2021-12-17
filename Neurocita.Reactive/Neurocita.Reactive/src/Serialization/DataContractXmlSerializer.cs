using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Neurocita.Reactive.Serialization
{
    public class DataContractXmlSerializer : ISerializer
    {
        private readonly DataContractSerializerSettings settings = new DataContractSerializerSettings();

        internal DataContractXmlSerializer(DataContractSerializerSettings settings)
        {
            this.settings = settings;
        }

        public string ContentType => "text/xml";

        public Stream Serialize<T>(T instance)
        {
            Type type = instance.GetType();
            (settings.KnownTypes as ISet<Type>).Add(type);

            DataContractSerializer serializer = new DataContractSerializer(type, settings);
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
            DataContractSerializer serializer = new DataContractSerializer(type, settings);
            return (T)serializer.ReadObject(stream);
        }
    }
}
