using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Xunit;

namespace THNETII.InteropServices.NativeMemory.Test
{
    [SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes")]
    public class SizeOfTest
    {
        [Fact]
        public void SizeOfBytesCorrect()
        {
            Assert.Equal(1, SizeOf<byte>.Bytes);
            Assert.Equal(8, SizeOf<byte>.Bits);
        }

        [Fact]
        public void SizeOfPointerCorrect()
        {
            Assert.Equal(IntPtr.Size, SizeOf<IntPtr>.Bytes);
        }

        [StructLayout(LayoutKind.Sequential)]
        class MarshalableTestType
        {
            public int test;
        }

        [Fact]
        public void SizeOfMarshalableType()
        {
            _ = SizeOf<MarshalableTestType>.Bytes;
        }

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
        class NonMarshalableTestType
        {
            public class NestedType
            {
                public object obj;
            }

            public NestedType instance;

            public object obj;

            public int @int;
        }
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value.

        [Fact]
        public void SizeOfNonMarshalableTypeThrowsException()
        {
            Assert.Throws<TypeInitializationException>(() =>
            {
                _ = SizeOf<NonMarshalableTestType>.Bytes;
            });
        }
    }
}
