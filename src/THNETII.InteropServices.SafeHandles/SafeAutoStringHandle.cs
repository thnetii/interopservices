using System;

namespace THNETII.InteropServices.SafeHandles
{
    public abstract class SafeAutoStringHandle : SafeStringHandle
    {
        public SafeAutoStringHandle(IntPtr invalidHandleValue, bool ownsHandle) : base(invalidHandleValue, ownsHandle) { }
    }
}
