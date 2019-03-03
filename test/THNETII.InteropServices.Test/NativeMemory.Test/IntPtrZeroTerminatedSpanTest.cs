using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Xunit;

namespace THNETII.InteropServices.NativeMemory.Test
{
    public static class IntPtrZeroTerminatedSpanTest
    {
        [Fact]
        public static void NullPointerToZeroTerminatedByteSpanReturnsEmpty()
        {
            var span = IntPtr.Zero.ToZeroTerminatedByteSpan();
            Assert.Equal(0, span.Length);
        }

        [Fact]
        public static void EmptyAnsiStringPointerToZeroTerminatedByteSpanReturnsEmpty()
        {
            var ptr = Marshal.StringToCoTaskMemAnsi(string.Empty);
            Assert.NotEqual(IntPtr.Zero, ptr);
            try
            {
                var span = ptr.ToZeroTerminatedByteSpan();
                Assert.Equal(0, span.Length);
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }

        [Fact]
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static unsafe void TenByteArrayPointerToZeroTerminatedByteSpanHasCorrectLength()
        {
            const int size = 10;
            var ptr = Marshal.AllocCoTaskMem(size + 1);
            Assert.NotEqual(IntPtr.Zero, ptr);
            try
            {
                var expected = new Span<byte>(ptr.ToPointer(), size);
                expected.Fill(42);
                Marshal.WriteByte(ptr, size, 0);

                var actual = ptr.ToZeroTerminatedByteSpan();

                Assert.Equal(size, actual.Length);
                for (int i = 0; i < size; i++)
                {
                    Assert.Equal(expected[i], actual[i]);
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }

        [Fact]
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static unsafe void MoreThanOnePageByteArrayPointerToZeroTerminatedByteSpanHasCorrectLength()
        {
            int size = Environment.SystemPageSize + (Environment.SystemPageSize / 2);
            var ptr = Marshal.AllocCoTaskMem(size);
            Assert.NotEqual(IntPtr.Zero, ptr);
            try
            {
                var expected = new Span<byte>(ptr.ToPointer(), size);
                expected.Fill(42);
                Marshal.WriteByte(ptr, size + 1, 0);

                var actual = ptr.ToZeroTerminatedByteSpan();

                Assert.Equal(size, actual.Length);
                for (int i = 0; i < size; i++)
                {
                    Assert.Equal(expected[i], actual[i]);
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }

        [Fact]
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static unsafe void MoreThanTwoGigaByteArrayPointerToZeroTerminatedByteSpanHasCorrectLength()
        {
            const int size1 = int.MaxValue;
            const int size2 = int.MaxValue / 2;
            const uint fullSize = (uint)size1 + (uint)size2;
            var ptr1 = Marshal.AllocCoTaskMem(unchecked((int)fullSize));
            Assert.NotEqual(IntPtr.Zero, ptr1);
            try
            {
                var ptr2 = ptr1 + size1;
                var ptrEnd = ptr2 + size2;

                var expected1 = new Span<byte>(ptr1.ToPointer(), size1);
                var expected2 = new Span<byte>(ptr2.ToPointer(), size2);

                expected1.Fill(42);
                expected2.Fill(42);
                Marshal.WriteByte(ptrEnd, 0);

                Assert.ThrowsAny<OverflowException>(() => ptr1.ToZeroTerminatedByteSpan());
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr1);
            }
        }

        [Fact]
        public static void NullPointerToZeroTerminatedUnicodeSpanReturnsEmpty()
        {
            var span = IntPtr.Zero.ToZeroTerminatedUnicodeSpan();
            Assert.Equal(0, span.Length);
        }

        [Fact]
        public static void EmptyUnicodeStringPointerToZeroTerminatedUnicodeSpanReturnsEmpty()
        {
            var ptr = Marshal.StringToCoTaskMemUni(string.Empty);
            Assert.NotEqual(IntPtr.Zero, ptr);
            try
            {
                var span = ptr.ToZeroTerminatedUnicodeSpan();
                Assert.Equal(0, span.Length);
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }

        [Fact]
        public static void TenCharStringPointerToZeroTerminatedUnicodeSpanHasCorrectLength()
        {
            string expected = new string('A', 10);
            var ptr = Marshal.StringToCoTaskMemUni(expected);
            Assert.NotEqual(IntPtr.Zero, ptr);
            try
            {
                var span = ptr.ToZeroTerminatedUnicodeSpan();
                Assert.Equal(expected.Length, span.Length);
                for (int i = 0; i < expected.Length; i++)
                {
                    Assert.Equal(expected[i], span[i]);
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }

        [Fact]
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static unsafe void NonAlignedMoreThanOnePageStringPointerToZeroTerminatedUnicodeSpanHasCorrectLength()
        {
            string expected = new string('A', Environment.SystemPageSize + (Environment.SystemPageSize / 2));
            int byteSize = (expected.Length + 1) * sizeof(char) + 1;
            var ptr = Marshal.AllocCoTaskMem(byteSize);
            Assert.NotEqual(IntPtr.Zero, ptr);
            try
            {
                var misalignedPtr = ptr;
                if (misalignedPtr.ToInt64() % 2 == 0)
                    misalignedPtr += 1;
                var expectedSpan = new Span<char>(misalignedPtr.ToPointer(), expected.Length + 1);
                expected.AsSpan().CopyTo(expectedSpan);
                expectedSpan[expectedSpan.Length - 1] = '\0';

                var span = misalignedPtr.ToZeroTerminatedUnicodeSpan();

                Assert.Equal(expected.Length, span.Length);
                for (int i = 0; i < expected.Length; i++)
                {
                    Assert.Equal(expected[i], span[i]);
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }

        [Fact]
        public static void MoreThanOnePageStringPointerToZeroTerminatedUnicodeSpanHasCorrectLength()
        {
            string expected = new string('A', Environment.SystemPageSize + (Environment.SystemPageSize / 2));
            var ptr = Marshal.StringToCoTaskMemUni(expected);
            Assert.NotEqual(IntPtr.Zero, ptr);
            try
            {
                var span = ptr.ToZeroTerminatedUnicodeSpan();
                Assert.Equal(expected.Length, span.Length);
                for (int i = 0; i < expected.Length; i++)
                {
                    Assert.Equal(expected[i], span[i]);
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }
    }
}
