using System;
using System.Collections.Generic;

namespace Neurocita.Reactive
{
    internal static class DataContractUtil
    {
        public static ISet<Type> PrepareKnownTypes(IEnumerable<Type> knownTypes)
        {
            ISet<Type> preparedKnownTypes;

            if (knownTypes == null)
                preparedKnownTypes = new HashSet<Type>();

            if (typeof(ISet<Type>).IsAssignableFrom(knownTypes.GetType()))
                preparedKnownTypes = knownTypes as ISet<Type>;
            else
                preparedKnownTypes = new HashSet<Type>(knownTypes);

            return preparedKnownTypes;
        }
    }
}
