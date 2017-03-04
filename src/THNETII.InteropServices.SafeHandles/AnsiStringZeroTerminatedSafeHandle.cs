using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public interface ISafeHandleReadableAsAnsiStringZeroTerminated : ISafeHandleReadableAs<string> { }

    public static class AnsiStringZeroTerminatedSafeHandle
    {
        public static string MarshalNativeToManaged(IntPtr pNativeData) => Marshal.PtrToStringAnsi(pNativeData);

        public static string ReadValue<THandle>(this THandle safeHandle)
            where THandle : SafeHandle, ISafeHandleReadableAsAnsiStringZeroTerminated
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

        public static string[] ReadValue<THandle>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleReadableAsAnsiStringZeroTerminatedArray
            => safeHandle.ReadValue(ptr => MarshalNativeToManaged(ptr, count));
    }
}
