using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    internal static class SizeOf<T> { public static readonly int Value = Marshal.SizeOf<T>(); }
}
