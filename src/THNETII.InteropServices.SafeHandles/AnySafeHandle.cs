using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public class AnySafeHandle : SafeHandle
    {
        private IntPtr invalidHandleValue;
        private SafeHandle owningHandle;
        private readonly bool owningHandleNeedRelease;

        public override bool IsInvalid => handle == invalidHandleValue;

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
                return false;
            else if (owningHandleNeedRelease)
                owningHandle?.DangerousRelease();
            return true;
        }

        protected AnySafeHandle() : this(ownsHandle: false) { }

        protected AnySafeHandle(bool ownsHandle) : this(IntPtr.Zero, ownsHandle) { }

        protected AnySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle)
        {
            this.invalidHandleValue = invalidHandleValue;
        }

        public AnySafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : this(invalidHandleValue, ownsHandle: true)
        {
            this.owningHandle = owningHandle;
            this.owningHandle.DangerousAddRef(ref owningHandleNeedRelease);
        }
    }
}