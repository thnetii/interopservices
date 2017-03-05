using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAs<out T> { }

    public static class SafeHandleExtensions
    {
        public static T ReadValue<T, THandle>(this THandle safeHandle, Func<IntPtr, T> marshalNativeToManaged)
            where THandle : ISafeHandleReadableAs<T>
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
