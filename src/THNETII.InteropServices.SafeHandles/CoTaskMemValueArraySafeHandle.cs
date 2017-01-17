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

        /// <param name="invalidHandleValue">The value of an invalid handle (usually <see cref="IntPtr.Zero"/>).</param>
        /// <param name="ownsHandle"><c>true</c> to reliably let the instance release the handle during the finalization phase; otherwise, <c>false</c> (not recommended).</param>
        protected CoTaskMemValueArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
    }
}
