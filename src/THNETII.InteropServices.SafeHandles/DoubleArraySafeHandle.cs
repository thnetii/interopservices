using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsDoubleArray : ISafeHandleReadableAs<double[]> { }

    public static class DoubleArraySafeHandle
    {
        public static double[] MarshalNativeToManaged(IntPtr pNativeData, int count)
        {
            if (pNativeData == IntPtr.Zero)
                return null;
            var array = new double[count];
            Marshal.Copy(source: pNativeData, destination: array, startIndex: 0, length: count);
            return array;

        }

        public static double[] ReadValue<THandle>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleReadableAsDoubleArray
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged(ptr, count));
    }
}
