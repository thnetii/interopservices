using System;
using System.Runtime.InteropServices;
using Xunit;

namespace THNETII.InteropServices.NativeMemory.Test
{
    public class IntPtrAsZeroTerminatedUnicodeSpanTest
    {
        [Fact]
        public void LPWStrNonEmptyAsSpan()
        {
            const string s = nameof(LPWStrNonEmptyAsSpan);
            var ptr = Marshal.StringToCoTaskMemUni(s);
            try
            {
                var span = ptr.AsZeroTerminatedUnicodeSpan();

                Assert.False(span.IsEmpty);
                Assert.Equal(s.Length, span.Length);
                Assert.Equal(s, span.ToString(), StringComparer.Ordinal);
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }

        [Fact]
        public void LPWStrEmptyAsSpan()
        {
            var ptr = Marshal.StringToCoTaskMemUni(string.Empty);
            try
            {
                var span = ptr.AsZeroTerminatedUnicodeSpan();

                Assert.True(span.IsEmpty);
                Assert.Equal(0, span.Length);
                Assert.Equal(string.Empty, span.ToString(), StringComparer.Ordinal);
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }

        [Fact]
        public void LPWStrNullAsSpanThrows()
        {
            var ptr = IntPtr.Zero;
            Assert.Throws<ArgumentNullException>(() => ptr.AsZeroTerminatedUnicodeSpan());
        }
    }
}
