using System;

namespace THNETII.InteropServices.SafeHandles
{
    public abstract class ValueArraySafeHandle<T> : SafeHandle<T[]>
    {
        public ValueArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle) : base(invalidHandleValue, ownsHandle) { }
    }
}
