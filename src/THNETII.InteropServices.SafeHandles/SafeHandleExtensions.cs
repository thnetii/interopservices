using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAs<out T> { }

    public static class SafeHandleExtensions
    {
        public static T ReadValue<T>(this ISafeHandleReadableAs<T> safeHandle, Func<IntPtr, T> marshalNativeToManaged)
        {
            if (safeHandle == null)
                throw new ArgumentNullException(nameof(safeHandle));
            SafeHandle safeHandleInstance;
            try { safeHandleInstance = safeHandle as SafeHandle; }
            catch (InvalidCastException invalidCastExcept) { throw new InvalidOperationException($"The specified safe handle with type {safeHandle.GetType()} does not inherit from the {typeof(SafeHandle)} class.", invalidCastExcept); }
            if (marshalNativeToManaged == null)
                throw new ArgumentNullException(nameof(marshalNativeToManaged));
            bool needsRelease = false;
            safeHandleInstance.DangerousAddRef(ref needsRelease);
            try { return marshalNativeToManaged(safeHandleInstance.DangerousGetHandle()); }
            finally
            {
                if (needsRelease)
                    safeHandleInstance.DangerousRelease();
            }
        }
    }
}
