using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsByte : ISafeHandleReadableAs<byte> { }

    public static class ByteSafeHandle
    {
        public static byte MarshalNativeToManaged(IntPtr pNativeData) => Marshal.ReadByte(pNativeData);

        public static byte ReadValue(this ISafeHandleReadableAsByte safeHandle)
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

        public static byte[] ReadValue(this ISafeHandleReadableAsByteArray safeHandle, int count)
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged(ptr, count));
    }

    public class ByteArrayAnySafeHandle : AnySafeHandle, ISafeHandleReadableAsByteArray
    {
        protected ByteArrayAnySafeHandle() : base() { }
        protected ByteArrayAnySafeHandle(bool ownsHandle) : base(ownsHandle) { }
        protected ByteArrayAnySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
        public ByteArrayAnySafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : base(invalidHandleValue, owningHandle) { }
    }

    public class ByteArrayCoTaskMemSafeHandle : CoTaskMemSafeHandle, ISafeHandleReadableAsByteArray
    {
        public ByteArrayCoTaskMemSafeHandle(byte[] value) : base(value?.Length ?? throw new ArgumentNullException(nameof(value)))
        {
            Marshal.Copy(source: value, destination: handle, startIndex: 0, length: value.Length);
        }

        public static implicit operator ByteArrayAnySafeHandle(ByteArrayCoTaskMemSafeHandle safeHandle)
            => safeHandle == null ? null : new ByteArrayAnySafeHandle(invalidHandleValue, safeHandle);
    }
}
