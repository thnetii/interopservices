using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public class CoTaskMemByteArraySafeHandle : CoTaskMemSafeHandle, ISafeHandleReadByteArray
    {
        public CoTaskMemByteArraySafeHandle(byte[] value)
            : base(value?.Length ?? throw new ArgumentNullException(nameof(value)))
        {
            Marshal.Copy(source: value, destination: handle, startIndex: 0, length: value.Length);
        }

        public static implicit operator ExternalByteArraySafeHandle(CoTaskMemByteArraySafeHandle coTaskMemSafeHandle)
        {
            if (coTaskMemSafeHandle == null)
                return null;
            return new ExternalByteArraySafeHandle(IntPtr.Zero, coTaskMemSafeHandle);
        }
    }
}
