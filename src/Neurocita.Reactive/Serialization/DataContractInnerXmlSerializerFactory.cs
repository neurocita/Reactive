using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Neurocita.Reactive.Serialization
{
    internal class DataContractInnerXmlSerializerFactory : IDataContractInnerSerializerFactory
    {
        private readonly DataContractSerializerSettings _settings;
        
        public DataContractInnerXmlSerializerFactory(DataContractSerializerSettings settings)
        {
            _settings = settings;
            _settings.KnownTypes = DataContractUtilities.PrepareKnownTypes(_settings.KnownTypes);
        }

        public ICollection<Type> KnownTypes => _settings.KnownTypes as ICollection<Type>;

    public XmlObjectSerializer Create(Type type) => this.CreateWithKnownType(type, (t) => new System.Runtime.Serialization.DataContractSerializer(t, _settings));
        }
}