using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace THNETII.InteropServices.SafeHandles
{
    public class ExternalByteArraySafeHandle : ExternalSafeHandle, ISafeHandleSizeUnawareReadByteArray
    {
        protected ExternalByteArraySafeHandle() : this(ownsHandle: false) { }
        protected ExternalByteArraySafeHandle(bool ownsHandle) : this(IntPtr.Zero, ownsHandle) { }
        protected ExternalByteArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
        public ExternalByteArraySafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : base(invalidHandleValue, owningHandle) { }
    }
}
