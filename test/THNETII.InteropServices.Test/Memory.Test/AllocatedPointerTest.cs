using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace THNETII.InteropServices.Memory.Test
{
    public static class AllocatedPointerTest
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct UnicodeStringPointer : IUnicodeStringPointer
        {
            public IntPtr Pointer { get; }
        }
    }
}
