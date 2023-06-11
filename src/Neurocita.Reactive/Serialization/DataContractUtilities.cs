using System;
using System.Collections.Generic;

namespace Neurocita.Reactive.Serialization
{
    public static class DataContractUtilities
    {
        internal static ICollection<Type> PrepareKnownTypes(this IEnumerable<Type> knownTypes)
        {

            if (knownTypes == null)
                return new HashSet<Type>();
            else  if((knownTypes as ICollection<Type>) == null)
                return new HashSet<Type>(knownTypes);
            else
                return knownTypes as ICollection<Type>;
        }
    }
}