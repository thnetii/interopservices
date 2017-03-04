using System;
using System.Linq;
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

    public class Int32AnySafeHandle : AnySafeHandle, ISafeHandleReadableAsInt32
    {
        protected Int32AnySafeHandle() : base() { }
        protected Int32AnySafeHandle(bool ownsHandle) : base(ownsHandle) { }
        protected Int32AnySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
        public Int32AnySafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : base(invalidHandleValue, owningHandle) { }
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

    public class Int32ArrayAnySafeHandle : AnySafeHandle, ISafeHandleReadableAsInt32Array
    {
        protected Int32ArrayAnySafeHandle() : base() { }
        protected Int32ArrayAnySafeHandle(bool ownsHandle) : base(ownsHandle) { }
        protected Int32ArrayAnySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
        public Int32ArrayAnySafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : base(invalidHandleValue, owningHandle) { }
    }

    public interface ISafeHandleReadableAsInt32CastArray<T> : ISafeHandleReadableAs<T[]> { }

    public static class Int32CastableArraySafeHandle
    {
        public static T[] MarshalNativeToManaged<T>(IntPtr pNativeData, int count)
        {
            return Int32ArraySafeHandle.MarshalNativeToManaged(pNativeData, count)
                ?.Cast<T>()
                .ToArray()
                ;
        }

        public static T[] ReadValue<THandle, T>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleReadableAsInt32CastArray<T>
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged<T>(ptr, count));
    }
}
