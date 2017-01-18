using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    public class StructPointerCoTaskMemMarshaler<T> : ICustomMarshaler
    {
        private static readonly int SizeOf = Marshal.SizeOf<T>();

        /// <summary>
        /// Performs necessary cleanup of the managed data when it is no longer needed.
        /// </summary>
        /// <param name="ManagedObj">The managed object to be destroyed.</param>
        public void CleanUpManagedData(object ManagedObj)
        {
            if (ManagedObj is IDisposable ManagedDisposable)
                ManagedDisposable.Dispose();
        }

        /// <summary>
        /// Performs necessary cleanup of the unmanaged data when it is no longer needed.
        /// </summary>
        /// <param name="pNativeData">A pointer to the unmanaged data to be destroyed. </param>
        public void CleanUpNativeData(IntPtr pNativeData) => Marshal.FreeCoTaskMem(pNativeData);

        /// <summary>
        /// Returns the size of the native data to be marshaled.
        /// </summary>
        /// <returns>The size, in bytes, of the native data.</returns>
        public int GetNativeDataSize() => -1;

        /// <summary>
        /// Converts the managed data to unmanaged data.
        /// </summary>
        /// <param name="ManagedObj">The managed object to be converted. </param>
        /// <returns>A pointer to the COM view of the managed object.</returns>
        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            if (ManagedObj is T ManagedInstance)
            {
                var pNativeData = Marshal.AllocCoTaskMem(SizeOf);
                Marshal.StructureToPtr(ManagedInstance, pNativeData, fDeleteOld: false);
                return pNativeData;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// Converts the unmanaged data to managed data.
        /// </summary>
        /// <param name="pNativeData">A pointer to the unmanaged data to be wrapped. </param>
        /// <returns>An object that represents the managed view of the COM data.</returns>
        public object MarshalNativeToManaged(IntPtr pNativeData) => Marshal.PtrToStructure<T>(pNativeData);

        public static StructPointerCoTaskMemMarshaler<T> GetInstance(string marshalCookie) => new StructPointerCoTaskMemMarshaler<T>();
    }
}
