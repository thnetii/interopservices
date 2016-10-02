using System;

namespace THNETII.InteropServices.SafeHandles
{
    public abstract class SafeStringHandle : SafeHandle<string>
    {
        public SafeStringHandle(IntPtr invalidHandleValue, bool ownsHandle) : base(invalidHandleValue, ownsHandle) { }
    }
}
