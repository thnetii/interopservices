using System;

namespace THNETII.InteropServices.SafeHandles
{
    public abstract class ReferenceArraySafeHandle<T> : SafeHandle<T[]>
    {
        protected ReferenceArraySafeHandle() : this(IntPtr.Zero) { }
        /// <param name="invalidHandleValue">The value of an invalid handle (usually <see cref="IntPtr.Zero"/>).</param>
        /// <param name="ownsHandle"><c>true</c> to reliably let the instance release the handle during the finalization phase; otherwise, <c>false</c> (not recommended).</param>
        protected ReferenceArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
    }
}
