using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsInt32 : ISafeHandleReadableAs<int> { }

    public static class Int32SafeHandle
    {
        public static int MarshalNativeToManaged(IntPtr pNativeData) => Marshal.ReadInt32(pNativeData);

        public static int ReadValue<THandle>(this THandle safeHandle)
            where THandle : SafeHandle, ISafeHandleReadableAsInt32
            => safeHandle.ReadValue(MarshalNativeToManaged);
    }

    public interface ISafeHandleReadableAsInt32Array : ISafeHandleReadableAs<int[]> { }

    public static class Int32ArraySafeHandle
    {
        public static int[] MarshalNativeToManaged(IntPtr pNativeData, int count)
        {
            if (pNativeData == IntPtr.Zero)
                return null;
            var array = new int[count];
            Marshal.Copy(source: pNativeData, destination: array, startIndex: 0, length: count);
            return array;
        }

        public static int[] ReadValue<THandle>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleReadableAsInt32Array
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged(ptr, count));
    }
}
