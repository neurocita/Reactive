using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Neurocita.Reactive.Serialization
{
    internal interface IDataContractInnerSerializerFactory
    {
        ICollection<Type> KnownTypes { get; }
        XmlObjectSerializer Create(Type type);
    }
}