namespace Neurocita.Reactive
{
    internal class ValueTypeDataContract<TValue> : IValueTypeDataContract<TValue>
        where TValue : struct
    {
        internal ValueTypeDataContract(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }

        public static implicit operator TValue(ValueTypeDataContract<TValue> dataContract) => dataContract.Value;
        public static explicit operator ValueTypeDataContract<TValue>(TValue value) => new ValueTypeDataContract<TValue>(value);

        public override bool Equals(object obj)
        {
            if (obj is ValueTypeDataContract<TValue> dataContract)
                return dataContract.Value.Equals(Value);

            if (obj.GetType() == typeof(TValue))
                return ((TValue)obj).Equals(Value);

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
