using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsAnsiStringZeroTerminated : ISafeHandleReadableAs<string> { }

    public static class AnsiStringZeroTerminatedSafeHandle
    {
        public static string MarshalNativeToManaged(IntPtr pNativeData) => Marshal.PtrToStringAnsi(pNativeData);

        public static string ReadValue(this ISafeHandleReadableAsAnsiStringZeroTerminated safeHandle)
            => safeHandle.ReadValue(MarshalNativeToManaged);
    }

    public interface ISafeHandleReadableAsAnsiStringZeroTerminatedArray : ISafeHandleReadableAs<string[]> { }

    public static class AnsiStringZeroTerminatedArraySafeHandle
    {
        public static string[] MarshalNativeToManaged(IntPtr pNativeData, int count)
        {
            return IntPtrArraySafeHandle.MarshalNativeToManaged(pNativeData, count)
                       ?.Select(ptr => AnsiStringZeroTerminatedSafeHandle.MarshalNativeToManaged(ptr))
                       .ToArray()
                       ;
        }

        public static string[] ReadValue(this ISafeHandleReadableAsAnsiStringZeroTerminatedArray safeHandle, int count)
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged(ptr, count));
    }
}
