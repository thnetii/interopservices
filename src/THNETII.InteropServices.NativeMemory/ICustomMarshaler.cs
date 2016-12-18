using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// Provides custom wrappers for handling method calls.
    /// </summary>
    public interface ICustomMarshaler
    {
        /// <summary>
        /// Performs necessary cleanup of the managed data when it is no longer needed.
        /// </summary>
        /// <param name="ManagedObj">The managed object to be destroyed.</param>
        void CleanUpManagedData(object ManagedObj);

        /// <summary>
        /// Performs necessary cleanup of the unmanaged data when it is no longer needed.
        /// </summary>
        /// <param name="pNativeData">A pointer to the unmanaged data to be destroyed. </param>
        void CleanUpNativeData(IntPtr pNativeData);

        /// <summary>
        /// Returns the size of the native data to be marshaled.
        /// </summary>
        /// <returns>The size, in bytes, of the native data.</returns>
        int GetNativeDataSize();

        /// <summary>
        /// Converts the managed data to unmanaged data.
        /// </summary>
        /// <param name="ManagedObj">The managed object to be converted. </param>
        /// <returns>A pointer to the COM view of the managed object.</returns>
        IntPtr MarshalManagedToNative(object ManagedObj);

        /// <summary>
        /// Converts the unmanaged data to managed data.
        /// </summary>
        /// <param name="pNativeData">A pointer to the unmanaged data to be wrapped. </param>
        /// <returns>An object that represents the managed view of the COM data.</returns>
        object MarshalNativeToManaged(IntPtr pNativeData);
    }
}
