namespace Neurocita.Reactive
{
    public interface ISerialization
    {
        string ContentType { get; }

        ISerializer CreateSerializer();
        IDeserializer CreateDeserializer();
    }
}
