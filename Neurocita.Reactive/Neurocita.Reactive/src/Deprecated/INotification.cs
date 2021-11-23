using System;

namespace Neurocita.Reactive
{
    public interface INotification
    {
        object Sender { get; }
        DateTimeOffset Timestamp { get; }
    }
}
