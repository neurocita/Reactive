namespace Neurocita.Reactive.Serialization
{
    public interface IValueTypeDataContract<TValue> : IDataContract
        where TValue : struct
    {
        TValue Value { get; }
    }
}
