using System.IO;

namespace Neurocita.Reactive
{
    public interface ITransportPipelineContext : IPipelineContext
    {
        IMessage<Stream> Message { get; }
        ISerialization Serialization { get; }
    }
}
