using System;
using System.Runtime.Serialization;

namespace Neurocita.Reactive.Serialization
{
    internal static class DataContractInnerSerializerFactoryExtensions
    {
        internal static XmlObjectSerializer CreateWithKnownType(this IDataContractInnerSerializerFactory factory, Type type, Func<Type,XmlObjectSerializer> implFactory)
        {
            factory.KnownTypes.Add(type);
            return implFactory.Invoke(type);
        }
    }
}