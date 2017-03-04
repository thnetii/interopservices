using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsInt64 : ISafeHandleReadableAs<long> { }

    public static class Int64SafeHandle
    {
        public static long MarshalNativeToManaged(IntPtr pNativeData) => Marshal.ReadInt64(pNativeData);

        public static long ReadValue<THandle>(this THandle safeHandle)
            where THandle : SafeHandle, ISafeHandleReadableAsInt64
            => safeHandle.ReadValue(MarshalNativeToManaged);
    }

    public interface ISafeHandleReadableAsInt64Array : ISafeHandleReadableAs<long[]> { }

    public static class Int64ArraySafeHandle
    {
        public static long[] MarshalNativeToManaged(IntPtr pNativeData, int count)
        {
            if (pNativeData == IntPtr.Zero)
                return null;
            var array = new long[count];
            Marshal.Copy(source: pNativeData, destination: array, startIndex: 0, length: count);
            return array;
        }

        public static long[] ReadValue<THandle>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleReadableAsInt64Array
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged(ptr, count));
    }
}
