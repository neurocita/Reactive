namespace Neurocita.Reactive.Configuration
{
    public interface INatsCanAddSecureOrUserCredentials
    {
        ICanAddSerializer Secure(bool secure);
        ICanAddSerializer WithUserCredentials(string credentialsPath);
        ICanAddSerializer WithUserCredentials(string jwt, string privateNkey);
    }
}