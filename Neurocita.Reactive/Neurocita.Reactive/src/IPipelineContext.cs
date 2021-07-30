using System.IO;

namespace Neurocita.Reactive
{
    public interface IPipelineContext : IRuntimeContext
    {
        IMessage<Stream> TransportMessage { get; }
    }
}
