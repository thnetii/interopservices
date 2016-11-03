using System;

namespace THNETII.InteropServices.SafeHandles
{
    public abstract class SafeWideStringHandle : SafeStringHandle
    {
        public SafeWideStringHandle(IntPtr invalidHandleValue, bool ownsHandle) : base(invalidHandleValue, ownsHandle) { }
    }
}
