using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsIntPtr : ISafeHandleReadableAs<IntPtr> { }

    public static class IntPtrSafeHandle
    {
        public static IntPtr MarshalNativeToManaged(IntPtr pNativeData) => Marshal.ReadIntPtr(pNativeData);

        public static IntPtr ReadValue<THandle>(this THandle safeHandle)
            where THandle : SafeHandle, ISafeHandleReadableAsIntPtr
            => safeHandle.ReadValue(MarshalNativeToManaged);
    }

    public interface ISafeHandleReadableAsIntPtrArray : ISafeHandleReadableAs<IntPtr[]> { }

    public static class IntPtrArraySafeHandle
    {
        public static IntPtr[] MarshalNativeToManaged(IntPtr pNativeData, int count)
        {
            if (pNativeData == IntPtr.Zero)
                return null;
            var array = new IntPtr[count];
            Marshal.Copy(source: pNativeData, destination: array, startIndex: 0, length: count);
            return array;
        }

        public static IntPtr[] ReadValue<THandle>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleReadableAsIntPtrArray
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged(ptr, count));
    }
}
