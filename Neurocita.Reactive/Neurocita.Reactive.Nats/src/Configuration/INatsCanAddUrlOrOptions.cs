using NATS.Client;

namespace Neurocita.Reactive.Configuration
{
    public interface INatsCanAddUrlOrOptions
    {
        INatsCanAddSecureOrUserCredentials WithUrl(string url);
        ICanAddSerializer WithOptions(Options options);
    }
}