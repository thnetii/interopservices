using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public class AnyGuidSafeHandle : AnySafeHandle, ISafeHandleReadableAsSimpleStructure<Guid>
    {
        protected AnyGuidSafeHandle() : base() { }
        protected AnyGuidSafeHandle(bool ownsHandle) : base(ownsHandle) { }
        protected AnyGuidSafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
        public AnyGuidSafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : base(invalidHandleValue, owningHandle) { }
    }

    public class CoTaskMemGuidSafeHandle : CoTaskMemSafeHandle, ISafeHandleReadableAsSimpleStructure<Guid>
    {
        public CoTaskMemGuidSafeHandle(Guid value) : base(SizeOf<Guid>.Value)
        {
            Marshal.StructureToPtr(value, handle, fDeleteOld: false);
        }

        public static implicit operator AnyGuidSafeHandle(CoTaskMemGuidSafeHandle safeHandle)
            => safeHandle == null ? null : new AnyGuidSafeHandle(invalidHandleValue, safeHandle);
    }
}
