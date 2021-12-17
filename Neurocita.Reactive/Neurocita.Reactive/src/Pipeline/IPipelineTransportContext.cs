using System.IO;

namespace Neurocita.Reactive.Pipeline
{
    public interface IPipelineTransportContext : IPipelineContext
    {
        IMessage<Stream> Message { get; }
    }
}
