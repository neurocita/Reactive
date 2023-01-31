using Neurocita.Reactive.Logging;

namespace Neurocita.Reactive.Configuration
{
    public interface ILoggingConfiguration
    {
        ILoggerFactory Factory { get; }    
    }
}
