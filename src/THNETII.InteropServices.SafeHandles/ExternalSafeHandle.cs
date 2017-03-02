using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public class ExternalSafeHandle : SafeHandle
    {
        private readonly IntPtr invalidHandleValue;
        private readonly SafeHandle owningHandle;
        private readonly bool owningHandleRequiresRelease;

        /// <inheritdoc />
        public override bool IsInvalid => handle == invalidHandleValue;

        protected override bool ReleaseHandle()
        {
            if (owningHandleRequiresRelease)
            {
                owningHandle.DangerousRelease();
                return true;
            }
            return false;
        }

        protected ExternalSafeHandle() : this(ownsHandle: false) { }
        protected ExternalSafeHandle(bool ownsHandle) : this(IntPtr.Zero, ownsHandle) { }
        protected ExternalSafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) :  base(invalidHandleValue, ownsHandle)
        {
            this.invalidHandleValue = invalidHandleValue;
        }

        public ExternalSafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : this(invalidHandleValue, ownsHandle: owningHandle != null)
        {
            this.owningHandle = owningHandle;
            owningHandle?.DangerousAddRef(ref owningHandleRequiresRelease);
            if (owningHandleRequiresRelease)
                handle = owningHandle.DangerousGetHandle();
        }
    }
}
