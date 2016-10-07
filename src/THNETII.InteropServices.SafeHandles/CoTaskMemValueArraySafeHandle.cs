using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace THNETII.InteropServices.SafeHandles
{
    public class CoTaskMemValueArraySafeHandle<T> : ValueArraySafeHandle<T>
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            var currentHandle = Interlocked.Exchange(ref handle, IntPtr.Zero);
            if (currentHandle == IntPtr.Zero)
                return false;
            Marshal.FreeCoTaskMem(handle);
            return true;
        }

        protected CoTaskMemValueArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle) : base(invalidHandleValue, ownsHandle) { }
    }
}
