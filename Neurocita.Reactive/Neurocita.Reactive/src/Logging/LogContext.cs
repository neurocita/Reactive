using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Neurocita.Reactive.Logging
{
    [Serializable]
    [DataContract]
    public class LogContext
    {
        private readonly IDictionary<string, ValueType> properties = new Dictionary<string, ValueType>();

        [DataMember]
        public IDictionary<string, ValueType> Properties => properties;
    }
}
