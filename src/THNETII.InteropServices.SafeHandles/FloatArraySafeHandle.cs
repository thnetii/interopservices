using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsFloatArray : ISafeHandleReadableAs<float[]> { }

    public static class FloatArraySafeHandle
    {
        public static float[] MarshalNativeToManaged(IntPtr pNativeData, int count)
        {
            if (pNativeData == IntPtr.Zero)
                return null;
            var array = new float[count];
            Marshal.Copy(source: pNativeData, destination: array, startIndex: 0, length: count);
            return array;
        }

        public static float[] ReadValue<THandle>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleReadableAsFloatArray
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged(ptr, count));
    }
}
