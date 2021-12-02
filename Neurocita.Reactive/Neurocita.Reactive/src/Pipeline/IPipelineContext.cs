namespace Neurocita.Reactive
{
    public enum PipelineDirection : byte
    {
        Inbound = 1,
        Outbound = 2
    }

    public interface IPipelineContext : IRuntimeContext
    {
        PipelineDirection Direction { get; }
    }
}
