using System;
using System.Runtime.Serialization;

namespace Neurocita.Reactive
{
    [Serializable]
    [DataContract]
    public class ValueTypeDataContract<TValue> : IValueTypeDataContract<TValue>
        where TValue : struct
    {
        public ValueTypeDataContract()
        {

        }

        public ValueTypeDataContract(TValue value)
        {
            Value = value;
        }

        [DataMember]
        public TValue Value { get; set; }

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
