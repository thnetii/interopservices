using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsWideStringZeroTerminated : ISafeHandleReadableAs<string> { }

    public static class WideStringZeroTerminatedSafeHandle
    {
        public static string MarshalNativeToManaged(IntPtr pNativeData) => Marshal.PtrToStringUni(pNativeData);

        public static string ReadValue(this ISafeHandleReadableAsWideStringZeroTerminated safeHandle)
            => safeHandle.ReadValue(MarshalNativeToManaged);
    }

    public class WideStringZeroTerminatedAnySafeHandle : AnySafeHandle, ISafeHandleReadableAsWideStringZeroTerminated
    {
        protected WideStringZeroTerminatedAnySafeHandle() : base() { }
        protected WideStringZeroTerminatedAnySafeHandle(bool ownsHandle) : base(ownsHandle) { }
        protected WideStringZeroTerminatedAnySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
        public WideStringZeroTerminatedAnySafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : base(invalidHandleValue, owningHandle) { }
    }

    public class WideStringZeroTerminatedCoTaskMemSafeHandle : CoTaskMemSafeHandle, ISafeHandleReadableAsWideStringZeroTerminated
    {
        public WideStringZeroTerminatedCoTaskMemSafeHandle(string value) : base(Marshal.StringToCoTaskMemUni(value)) { }

        public static implicit operator WideStringZeroTerminatedAnySafeHandle(WideStringZeroTerminatedCoTaskMemSafeHandle safeHandle)
            => safeHandle == null ? null : new WideStringZeroTerminatedAnySafeHandle(invalidHandleValue, safeHandle);
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

        public static string[] ReadValue(this ISafeHandleReadableAsWideStringZeroTerminatedArray safeHandle, int count)
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged(ptr, count));
    }

    public class WideStringZeroTerminatedArrayAnySafeHandle : AnySafeHandle, ISafeHandleReadableAsWideStringZeroTerminatedArray
    {
        protected WideStringZeroTerminatedArrayAnySafeHandle() : base() { }
        protected WideStringZeroTerminatedArrayAnySafeHandle(bool ownsHandle) : base(ownsHandle) { }
        protected WideStringZeroTerminatedArrayAnySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
        public WideStringZeroTerminatedArrayAnySafeHandle(IntPtr invalidHandleValue, SafeHandle owningHandle) : base(invalidHandleValue, owningHandle) { }
    }
}
