using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    internal static class DataContractUtil
    {
        public static ISet<Type> PrepareKnownTypes(IEnumerable<Type> knownTypes)
        {
            return typeof(ISet<Type>).IsAssignableFrom(knownTypes.GetType())
                    ? knownTypes as ISet<Type>
                    : new HashSet<Type>(knownTypes);
        }
    }
}
