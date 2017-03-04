using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsByte : ISafeHandleReadableAs<byte> { }

    public static class ByteSafeHandle
    {
        public static byte MarshalNativeToManaged(IntPtr pNativeData) => Marshal.ReadByte(pNativeData);

        public static byte ReadValue<THandle>(this THandle safeHandle)
            where THandle : SafeHandle, ISafeHandleReadableAsByte
            => safeHandle.ReadValue(MarshalNativeToManaged);
    }

    public interface ISafeHandleReadableAsByteArray : ISafeHandleReadableAs<byte[]> { }

    public static class ByteArraySafeHandle
    {
        public static byte[] MarshalNativeToManaged(IntPtr pNativeData, int count)
        {
            if (pNativeData == IntPtr.Zero)
                return null;
            var byteArray = new byte[count];
            Marshal.Copy(source: pNativeData, destination: byteArray, startIndex: 0, length: count);
            return byteArray;
        }

        public static byte[] ReadValue<THandle>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleReadableAsByteArray
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged(ptr, count));
    }

    public class AnyByteArraySafeHandle : AnySafeHandle, ISafeHandleReadableAsByteArray
    {
        protected AnyByteArraySafeHandle() : base() { }
        protected AnyByteArraySafeHandle(bool ownsHandle) : base(ownsHandle) { }
        protected AnyByteArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
        public AnyByteArraySafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : base(invalidHandleValue, owningHandle) { }
    }

    public class CoTaskMemByteArraySafeHandle : CoTaskMemSafeHandle, ISafeHandleReadableAsByteArray
    {
        public CoTaskMemByteArraySafeHandle(byte[] value) : base(value?.Length ?? throw new ArgumentNullException(nameof(value)))
        {
            Marshal.Copy(source: value, destination: handle, startIndex: 0, length: value.Length);
        }

        public static implicit operator AnyByteArraySafeHandle(CoTaskMemByteArraySafeHandle safeHandle)
            => safeHandle == null ? null : new AnyByteArraySafeHandle(invalidHandleValue, safeHandle);
    }
}
