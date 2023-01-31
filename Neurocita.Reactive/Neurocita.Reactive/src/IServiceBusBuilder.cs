using System;
using System.Collections.Generic;
using System.Text;
using Neurocita.Reactive.Configuration;
using Neurocita.Reactive.Transport;
using Neurocita.Reactive.Serialization;

namespace Neurocita.Reactive
{
    public interface IServiceBusBuilder
    {
        TTransport Transport<TTransport>() where TTransport : ITransport;
        TSerializer Serializer<TSerializer>() where TSerializer : ISerializer;
        IServiceBus Build();
    }
}
