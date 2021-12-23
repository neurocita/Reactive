namespace Neurocita.Reactive.Configuration
{
    public interface ICanAddEndpoint
    {
        ICanAddEndpointOrCreate WithEndpoint(string name, string nodePath);
    }
}