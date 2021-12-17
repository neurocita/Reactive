namespace Neurocita.Reactive.Configuration
{
    public class EndpointConfiguration : IEndpointConfiguration
    {
        private readonly string nodePath;

        public EndpointConfiguration(string nodePath)
        {
            this.nodePath = nodePath;
        }

        public string NodePath => nodePath;
    }
}