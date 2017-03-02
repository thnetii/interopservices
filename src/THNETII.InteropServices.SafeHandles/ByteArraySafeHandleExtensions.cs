using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public static class ByteArraySafeHandleExtensions
    {
        private static byte[] ReadByteArray(this SafeHandle safeHandle, int byteSize)
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
                var byteArray = byteSize < 0 ? new byte[(uint)byteSize] : new byte[byteSize];
                Marshal.Copy(source: safeHandle.DangerousGetHandle(), destination: byteArray, startIndex: 0, length: byteArray.Length);
                return byteArray;
            }
            finally { safeHandle.DangerousRelease(); }
        }

        public static byte[] ReadValue<THandle>(this THandle safeHandle)
            where THandle : SafeHandle, ISafeHandleReadByteArray
            => safeHandle.ReadByteArray(safeHandle.ByteSize);

        public static byte[] ReadValue<THandle>(this THandle safeHandle, int byteSize)
            where THandle : SafeHandle, ISafeHandleSizeUnawareReadByteArray
            => safeHandle.ReadByteArray(byteSize);
    }
}
