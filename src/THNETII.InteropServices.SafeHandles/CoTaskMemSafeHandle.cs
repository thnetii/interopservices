using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public class CoTaskMemSafeHandle : SafeHandle, ISafeHandleSizeAware
    {
        private int byteSize;

        /// <inheritdoc />
        public override bool IsInvalid => handle == IntPtr.Zero;

        public int ByteSize
        {
            get { return byteSize; }
        }

        /// <inheritdoc />
        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
                return false;
            Marshal.FreeCoTaskMem(handle);
            return true;
        }

        protected CoTaskMemSafeHandle() : this(ownsHandle: false) { }
        protected CoTaskMemSafeHandle(bool ownsHandle) : this(IntPtr.Zero, ownsHandle) { }
        protected CoTaskMemSafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }

        public CoTaskMemSafeHandle(int byteSize) : this(ownsHandle: true)
        {
            handle = Marshal.AllocCoTaskMem(byteSize);
            this.byteSize = byteSize;
        }
    }
}
