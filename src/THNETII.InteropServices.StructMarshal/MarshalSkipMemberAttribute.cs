using System;

namespace THNETII.InteropServices.StructMarshal
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class MarshalSkipMemberAttribute : Attribute { }
}
