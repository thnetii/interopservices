using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public class CoTaskMemSafeHandle : SafeHandle
    {
        protected static readonly IntPtr invalidHandleValue = IntPtr.Zero;

        public override bool IsInvalid => handle == invalidHandleValue;

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
                return false;
            Marshal.FreeCoTaskMem(handle);
            return true;
        }

        protected CoTaskMemSafeHandle(IntPtr coTaskMemHandle) : base(invalidHandleValue, ownsHandle: true)
        {
            handle = coTaskMemHandle;
        }

        public CoTaskMemSafeHandle(int byteSize) : this(Marshal.AllocCoTaskMem(byteSize)) { }

        public static implicit operator AnySafeHandle(CoTaskMemSafeHandle safeHandle)
            => safeHandle == null ? null : new AnySafeHandle(invalidHandleValue, safeHandle);
    }
}
