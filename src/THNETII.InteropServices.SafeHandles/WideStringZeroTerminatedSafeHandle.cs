using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsWideStringZeroTerminated : ISafeHandleReadableAs<string> { }

    public static class WideStringZeroTerminatedSafeHandle
    {
        public static string MarshalNativeToManaged(IntPtr pNativeData) => Marshal.PtrToStringUni(pNativeData);

        public static string ReadValue<THandle>(this THandle safeHandle)
            where THandle : SafeHandle, ISafeHandleReadableAsWideStringZeroTerminated
            => safeHandle.ReadValue(MarshalNativeToManaged);
    }

    public class AnyWideStringZeroTerminatedSafeHandle : AnySafeHandle, ISafeHandleReadableAsWideStringZeroTerminated
    {
        protected AnyWideStringZeroTerminatedSafeHandle() : base() { }
        protected AnyWideStringZeroTerminatedSafeHandle(bool ownsHandle) : base(ownsHandle) { }
        protected AnyWideStringZeroTerminatedSafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
        public AnyWideStringZeroTerminatedSafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : base(invalidHandleValue, owningHandle) { }
    }

    public class CoTaskMemWideStringZeroTerminatedSafeHandle : CoTaskMemSafeHandle, ISafeHandleReadableAsWideStringZeroTerminated
    {
        public CoTaskMemWideStringZeroTerminatedSafeHandle(string value) : base(Marshal.StringToCoTaskMemUni(value)) { }

        public static implicit operator AnyWideStringZeroTerminatedSafeHandle(CoTaskMemWideStringZeroTerminatedSafeHandle safeHandle)
            => safeHandle == null ? null : new AnyWideStringZeroTerminatedSafeHandle(invalidHandleValue, safeHandle);
    }

    public interface ISafeHandleReadableAsWideStringZeroTerminatedArray : ISafeHandleReadableAs<string[]> { }

    public static class WideStringZeroTerminatedArraySafeHandle
    {
        public static string[] MarshalNativeToManaged(IntPtr pNativeData, int count)
        {
            return IntPtrArraySafeHandle.MarshalNativeToManaged(pNativeData, count)
                       ?.Select(ptr => WideStringZeroTerminatedSafeHandle.MarshalNativeToManaged(ptr))
                       .ToArray()
                       ;
        }

        public static string[] ReadValue<THandle>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleReadableAsWideStringZeroTerminatedArray
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged(ptr, count));
    }
}
