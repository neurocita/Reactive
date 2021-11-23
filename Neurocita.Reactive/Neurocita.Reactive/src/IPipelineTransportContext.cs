using System.IO;

namespace Neurocita.Reactive
{
    public interface IPipelineTransportContext : IPipelineContext
    {
        IMessage<Stream> Message { get; }
    }
}
