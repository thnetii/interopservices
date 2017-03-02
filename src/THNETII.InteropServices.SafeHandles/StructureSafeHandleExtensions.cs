using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public static class StructureSafeHandleExtensions
    {
        internal static T ReadStructure<T>(this SafeHandle safeHandle)
            where T : new()
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
                return Marshal.PtrToStructure<T>(safeHandle.DangerousGetHandle());
            }
            finally { safeHandle.DangerousRelease(); }
        }

        public static T ReadValue<THandle, T>(this THandle safeHandle)
            where T : new()
            where THandle : SafeHandle, ISafeHandleReadStructure<T>
            => safeHandle.ReadStructure<T>();
    }
}
