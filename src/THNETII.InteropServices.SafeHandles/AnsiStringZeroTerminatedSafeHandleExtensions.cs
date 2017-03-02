using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public static class AnsiStringZeroTerminatedSafeHandleExtensions
    {
        internal static string ReadAnsiStringZeroTerminated(this SafeHandle safeHandle)
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
                return Marshal.PtrToStringAnsi(safeHandle.DangerousGetHandle());
            }
            finally { safeHandle.DangerousRelease(); }
        }

        public static string ReadValue<THandle>(this THandle safeHandle)
            where THandle : SafeHandle, ISafeHandleSizeUnawareReadAnsiStringZeroTerminated
            => safeHandle.ReadAnsiStringZeroTerminated();
    }
}
