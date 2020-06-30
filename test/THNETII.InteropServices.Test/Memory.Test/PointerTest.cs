using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace THNETII.InteropServices.Memory.Test
{
    public static class PointerTest
    {
        private struct IntArrayPointer : IArrayPointer<int>
        {
            public IntPtr Pointer { get; }
        }

        [Fact]
        public unsafe static void Can_create_pointer_from_IntPtr()
        {
            const int length = 10;
            int* unsafeptr = stackalloc int[length];
            var arrayptr = Pointer.Create<IntArrayPointer>(new IntPtr(unsafeptr));
        }

        [Fact]
        public unsafe static void Pointer_to_null_has_no_value()
        {
            var arrayptr = Pointer.Create<IntArrayPointer>(IntPtr.Zero);
            Assert.False(arrayptr.HasValue());
        }
    }
}
