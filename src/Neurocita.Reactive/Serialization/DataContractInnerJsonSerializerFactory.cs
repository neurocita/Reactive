using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Neurocita.Reactive.Serialization
{
    internal class DataContractInnerJsonSerializerFactory : IDataContractInnerSerializerFactory
    {
        private readonly DataContractJsonSerializerSettings _settings;
        
        public DataContractInnerJsonSerializerFactory(DataContractJsonSerializerSettings settings)
        {
            _settings = settings;
            _settings.KnownTypes = DataContractUtilities.PrepareKnownTypes(_settings.KnownTypes);
        }

        public ICollection<Type> KnownTypes => _settings.KnownTypes as ICollection<Type>;

        public XmlObjectSerializer Create(Type type) => this.CreateWithKnownType(type, (t) => new System.Runtime.Serialization.Json.DataContractJsonSerializer(t, _settings));
    }
}