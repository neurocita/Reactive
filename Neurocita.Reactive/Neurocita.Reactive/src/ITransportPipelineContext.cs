using System.IO;

namespace Neurocita.Reactive
{
    public interface ITransportPipelineContext
    {
        IRuntimeContext Runtime { get; }
        IMessage<Stream> Message { get; }
    }
}
