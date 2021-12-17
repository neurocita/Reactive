namespace Neurocita.Reactive.Configuration
{
    public interface ICanAddEndpoint
    {
        ICanAddEndpointOrBuild WithEndpoint(string name, string nodePath);
    }
}