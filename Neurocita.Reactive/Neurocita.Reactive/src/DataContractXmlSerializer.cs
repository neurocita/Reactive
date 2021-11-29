using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Neurocita.Reactive
{
    public class DataContractXmlSerializer : ISerializer
    {
        private readonly DataContractSerializerSettings settings = new DataContractSerializerSettings();

        public DataContractXmlSerializer()
            : this(new HashSet<Type>())
        {

        }

        public DataContractXmlSerializer(IEnumerable<Type> knownTypes)
        {
            settings.KnownTypes = DataContractUtil.PrepareKnownTypes(knownTypes);
        }

        public DataContractXmlSerializer(DataContractSerializerSettings settings)
        {
            this.settings = settings;
            this.settings.KnownTypes = DataContractUtil.PrepareKnownTypes(this.settings.KnownTypes);
        }

        public DataContractSerializerSettings Settings => settings;
        public string ContentType => "text/xml";

        public Stream Serialize<T>(T instance)
        {
            this.settings.KnownTypes = DataContractUtil.PrepareKnownTypes(this.settings.KnownTypes);
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
            this.settings.KnownTypes = DataContractUtil.PrepareKnownTypes(this.settings.KnownTypes);
            Type type = typeof(T);
            (settings.KnownTypes as ISet<Type>).Add(type);

            stream.Position = 0;
            DataContractSerializer serializer = new DataContractSerializer(type, settings);
            return (T)serializer.ReadObject(stream);
        }
    }
}
