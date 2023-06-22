using System.Collections.Generic;

namespace Neurocita.Reactive.Utilities
{
    public static class EnumerableExtensions
    {
        public static ICollection<T> ToCollection<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return new HashSet<T>();
            else if((enumerable as ICollection<T>) == null)
                return new HashSet<T>(enumerable);
            else
                return enumerable as ICollection<T>;
        }
    }
}