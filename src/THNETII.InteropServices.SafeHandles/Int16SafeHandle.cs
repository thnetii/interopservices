using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsInt16 : ISafeHandleReadableAs<short> { }

    public static class Int16SafeHandle
    {
        public static short MarshalNativeToManaged(IntPtr pNativeData) => Marshal.ReadInt16(pNativeData);

        public static short ReadValue(this ISafeHandleReadableAsInt16 safeHandle)
            => safeHandle.ReadValue(MarshalNativeToManaged);
    }

    public interface ISafeHandleReadableAsInt16Array : ISafeHandleReadableAs<short[]> { }

    public static class Int16ArraySafeHandle
    {
        public static short[] MarshalNativeToManaged(IntPtr pNativeData, int count)
        {
            if (pNativeData == IntPtr.Zero)
                return null;
            var array = new short[count];
            Marshal.Copy(source: pNativeData, destination: array, startIndex: 0, length: count);
            return array;
        }

        public static short[] ReadValue(this ISafeHandleReadableAsInt16Array safeHandle, int count)
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged(ptr, count));
    }
}
