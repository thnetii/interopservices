using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsSimpleStructure<out T> : ISafeHandleReadableAs<T> { }

    public static class SimpleStructureSafeHandle
    {
        public static T MarshalNativeToManaged<T>(IntPtr pNativeData) => Marshal.PtrToStructure<T>(pNativeData);

        public static T ReadValue<THandle, T>(this THandle safeHandle)
            where THandle : SafeHandle, ISafeHandleReadableAsSimpleStructure<T>
            => safeHandle.ReadValue(MarshalNativeToManaged<T>);
    }

    public interface ISafeHandleReadableAsSimpleStructureArray<T> : ISafeHandleReadableAs<T[]> { }

    public static class SimpleStructureArraySafeHandle
    {
        public static T[] MarshalNativeToManaged<T>(IntPtr pNativeData, int count)
        {
            if (pNativeData == IntPtr.Zero)
                return null;
            var array = new T[count];
            int i = 0;
            for (var pNativeCurrent = pNativeData; i < count; pNativeCurrent += SizeOf<T>.Value, i++)
                array[i] = Marshal.PtrToStructure<T>(pNativeCurrent);
            return array;
        }

        public static T[] ReadValue<THandle, T>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleReadableAsSimpleStructureArray<T>
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged<T>(ptr, count));
    }

    public interface ISafeHandleReadableAsSimpleStructureReferenceArray<T> : ISafeHandleReadableAs<T[]> { }

    public static class SimpleStructureReferenceArraySafeHandle
    {
        public static T[] MarshalNativeToManaged<T>(IntPtr pNativeData, int count)
        {
            return IntPtrArraySafeHandle.MarshalNativeToManaged(pNativeData, count)
                ?.Select(ptr => Marshal.PtrToStructure<T>(ptr))
                .ToArray()
                ;
        }

        public static T[] ReadValue<THandle, T>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleReadableAsSimpleStructureReferenceArray<T>
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged<T>(ptr, count));
    }
}
