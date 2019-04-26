using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Xunit;

namespace THNETII.InteropServices.Memory.Test
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
        public void SizeOfCharCorrect() => Assert.Equal(2, SizeOf<char>.Bytes);

        [Fact]
        public static void SizeOfInt16Correct()
        {
            Assert.Equal(2, SizeOf<short>.Bytes);
            Assert.Equal(16, SizeOf<short>.Bits);
        }

        [Fact]
        public static void SizeOfInt32Correct()
        {
            Assert.Equal(4, SizeOf<int>.Bytes);
            Assert.Equal(32, SizeOf<int>.Bits);
        }

        [Fact]
        public static void SizeOfInt64Correct()
        {
            Assert.Equal(8, SizeOf<long>.Bytes);
            Assert.Equal(64, SizeOf<long>.Bits);
        }

        [Fact]
        public static void SizeOfGuidCorrect() => Assert.Equal(128, SizeOf<Guid>.Bits);

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
