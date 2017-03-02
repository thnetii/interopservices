using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public static class IntPtrArraySafeHandleExtensions
    {
        internal static IntPtr[] ReadIntPtrArray(this SafeHandle safeHandle, int count)
        {
            if (safeHandle == null)
                throw new ArgumentNullException(nameof(safeHandle));
            else if (safeHandle.IsClosed)
                throw new ObjectDisposedException(nameof(safeHandle));
            else if (safeHandle.IsInvalid)
                throw new InvalidOperationException("The specified safe handle is an invalid handle.");
            bool needsSafeHandleRelease = false;
            safeHandle.DangerousAddRef(ref needsSafeHandleRelease);
            try
            {
                var intPtrArray = count < 0 ? new IntPtr[(uint)count] : new IntPtr[count];
                Marshal.Copy(source: safeHandle.DangerousGetHandle(), destination: intPtrArray, startIndex: 0, length: count);
                return intPtrArray;
            }
            finally { safeHandle.DangerousRelease(); }
        }

        public static IntPtr[] ReadValue<THandle>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleSizeUnawareReadIntPtrArray
            => safeHandle.ReadIntPtrArray(count);
    }
}
