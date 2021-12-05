namespace Neurocita.Reactive
{
    public interface IValueTypeDataContract<TValue> : IDataContract
        where TValue : struct
    {
        TValue Value { get; }
    }
}
