namespace Neurocita.Reactive
{
    public static class MessageHeaders
    {
        public const string MessageId = "x-neurocita-message-id";
        public const string UserId = "x-neurocita-user-id";
        public const string ContentSourceType = "x-neurocita-content-source-type";
        public const string QualifiedTypeName = "x-neurocita-qualified-type-name";
        public const string Subject = "x-neurocita-subject";
        public const string To = "x-neurocita-to";
        public const string ReplyTo = "x-neurocita-reply-to";
        public const string CorrelationId = "x-neurocita-correlation-id";
        public const string RfcContentType = "x-neurocita-rfc-content-type";
        public const string ContentEncoding = "x-neurocita-content-encoding";
        public const string ExpiryTime = "x-neurocita-expiry-time";
        public const string CreationTime = "x-neurocita-creationn-time";
        public const string Ttl = "x-neurocita-ttl";
        public const string ObservableEvent = "x-neurocita-observable-event";       // OnNext, OnError, OnCompleted
    }
}
