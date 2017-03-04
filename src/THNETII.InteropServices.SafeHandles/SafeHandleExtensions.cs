using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAs<T> { }

    public static class SafeHandleExtensions
    {
        public static T ReadValue<T, THandle>(this THandle safeHandle, Func<IntPtr, T> marshalNativeToManaged)
            where THandle : SafeHandle, ISafeHandleReadableAs<T>
        {
            if (safeHandle == null)
                throw new ArgumentNullException(nameof(safeHandle));
            else if (marshalNativeToManaged == null)
                throw new ArgumentNullException(nameof(marshalNativeToManaged));
            bool needsRelease = false;
            safeHandle.DangerousAddRef(ref needsRelease);
            try { return marshalNativeToManaged(safeHandle.DangerousGetHandle()); }
            finally
            {
                if (needsRelease)
                    safeHandle.DangerousRelease();
            }
        }
    }
}
