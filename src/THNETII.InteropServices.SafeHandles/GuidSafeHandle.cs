using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public class GuidAnySafeHandle : AnySafeHandle, ISafeHandleReadableAsSimpleStructure<Guid>
    {
        protected GuidAnySafeHandle() : base() { }
        protected GuidAnySafeHandle(bool ownsHandle) : base(ownsHandle) { }
        protected GuidAnySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
        public GuidAnySafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : base(invalidHandleValue, owningHandle) { }
    }

    public class GuidCoTaskMemSafeHandle : CoTaskMemSafeHandle, ISafeHandleReadableAsSimpleStructure<Guid>
    {
        public GuidCoTaskMemSafeHandle(Guid value) : base(SizeOf<Guid>.Value)
        {
            Marshal.StructureToPtr(value, handle, fDeleteOld: false);
        }

        public static implicit operator GuidAnySafeHandle(GuidCoTaskMemSafeHandle safeHandle)
            => safeHandle == null ? null : new GuidAnySafeHandle(invalidHandleValue, safeHandle);
    }
}
