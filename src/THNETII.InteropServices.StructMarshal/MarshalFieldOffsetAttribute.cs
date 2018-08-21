using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.StructMarshal
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class MarshalFieldOffsetAttribute : Attribute
    {
        public MarshalFieldOffsetAttribute(int offset)
        {
            Value = offset;
        }

        public int Value { get; }

        public static explicit operator FieldOffsetAttribute(MarshalFieldOffsetAttribute marshalFieldOffsetAttribute)
        {
            if (marshalFieldOffsetAttribute == null)
                return null;
            return new FieldOffsetAttribute(marshalFieldOffsetAttribute.Value);
        }

        public static explicit operator MarshalFieldOffsetAttribute(FieldOffsetAttribute fieldOffsetAttribute)
        {
            if (fieldOffsetAttribute == null)
                return null;
            return new MarshalFieldOffsetAttribute(fieldOffsetAttribute.Value);
        }
    }
}
