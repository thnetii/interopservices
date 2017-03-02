using System.Linq;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public static class WideStringZeroTerminatedArraySafeHandleExtensions
    {
        public static string[] ReadValue<THandle>(this THandle safeHandle, int count)
            where THandle : SafeHandle, ISafeHandleSizeUnawareReadWideStringZeroTerminatedArray
            => safeHandle.ReadIntPtrArray(count)?.Select(ptr => Marshal.PtrToStringUni(ptr)).ToArray();
    }
}
