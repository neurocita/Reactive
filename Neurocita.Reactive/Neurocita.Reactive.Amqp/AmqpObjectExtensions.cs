using Amqp;
using System;
using System.Reflection;

namespace Neurocita.Reactive.Amqp
{
    internal static class AmqpObjectExtensions
    {
        internal static bool IsClosedHandled(this IAmqpObject amqpObject)
        {
            //ClosedCallback closedCallback = amqpObject.Closed;
            //return closedCallback != null;

            const string eventName = "Closed";

            Type classType = amqpObject.GetType();
            FieldInfo eventField = classType.GetField(eventName, BindingFlags.GetField
                                                               | BindingFlags.NonPublic
                                                               | BindingFlags.Instance);
            return eventField?.GetValue(amqpObject) is ClosedCallback;
        }
    }
}
