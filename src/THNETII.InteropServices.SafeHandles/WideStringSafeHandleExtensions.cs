using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public static class WideStringSafeHandleExtensions
    {
        private static string ReadWideStringZeroTerminated(this SafeHandle safeHandle)
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
                return Marshal.PtrToStringUni(safeHandle.DangerousGetHandle());
            }
            finally { safeHandle.DangerousRelease(); }
        }

        public static string ReadValue<THandle>(this THandle safeHandle)
            where THandle : SafeHandle, ISafeHandleSizeUnawareReadWideStringZeroTerminated
            => safeHandle.ReadWideStringZeroTerminated();
    }
}
