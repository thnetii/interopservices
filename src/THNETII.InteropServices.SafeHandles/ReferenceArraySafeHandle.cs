using System;

namespace THNETII.InteropServices.SafeHandles
{
    public abstract class ReferenceArraySafeHandle<T> : SafeHandle<T[]>
    {
        public ReferenceArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle) : base(invalidHandleValue, ownsHandle) { }
    }
}
