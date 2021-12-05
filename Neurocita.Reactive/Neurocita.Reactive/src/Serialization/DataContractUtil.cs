using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    internal static class DataContractUtil
    {
        public static ISet<Type> PrepareKnownTypes(IEnumerable<Type> knownTypes)
        {
            IEnumerable<Type> knownTypesLocal = knownTypes == null ? new HashSet<Type>() : knownTypes;

            return typeof(ISet<Type>).IsAssignableFrom(knownTypesLocal.GetType())
                    ? knownTypesLocal as ISet<Type>
                    : new HashSet<Type>(knownTypes);
        }
    }
}
