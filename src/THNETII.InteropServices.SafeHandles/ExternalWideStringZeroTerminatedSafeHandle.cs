using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public class ExternalWideStringZeroTerminatedSafeHandle : ExternalSafeHandle, ISafeHandleSizeUnawareReadWideStringZeroTerminated
    {
        protected ExternalWideStringZeroTerminatedSafeHandle() : this(ownsHandle: false) { }
        protected ExternalWideStringZeroTerminatedSafeHandle(bool ownsHandle) : this(IntPtr.Zero, ownsHandle) { }
        protected ExternalWideStringZeroTerminatedSafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
        public ExternalWideStringZeroTerminatedSafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : base(invalidHandleValue, owningHandle) { }
    }
}
