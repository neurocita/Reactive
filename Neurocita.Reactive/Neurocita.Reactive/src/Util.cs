using System;

namespace Neurocita.Reactive
{
    public static class Util
    {
        public static void CheckNullArgument<T>(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(typeof(T).FullName);
        }
    }
}
