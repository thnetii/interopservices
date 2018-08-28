using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("Style", "IDE0044:Add readonly modifier")]
    public class UntypedPointer
    {
        private IntPtr ptr;

        public IntPtr Pointer => ptr;
    }
}
