using System.IO;

namespace Neurocita.Reactive
{
    public interface ITransportPipelineContext : IRuntimeContext
    {
        IMessage<Stream> TransportMessage { get; }
    }
}
