using System;

namespace THNETII.InteropServices.StructMarshal
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class MarshalFieldOrderAttribute : Attribute
    {
        public MarshalFieldOrderAttribute(int order)
        {
            if (order < 0)
                throw new ArgumentOutOfRangeException(nameof(order), order, "Value must not be negative.");
            Value = order;
        }

        public int Value { get; }
    }
}
