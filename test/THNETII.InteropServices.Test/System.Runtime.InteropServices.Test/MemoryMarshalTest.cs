using System.Runtime.CompilerServices;
using Xunit;

namespace System.Runtime.InteropServices.Test
{
    public class MemoryMarshalTest
    {
        ReadOnlySpan<byte> AsBytes<T>(in T readOnly) where T : unmanaged
        {
            ref T reference = ref Unsafe.AsRef(readOnly);
            ReadOnlySpan<T> span = MemoryMarshal.CreateReadOnlySpan(ref reference, length: 1);
            return MemoryMarshal.AsBytes(span);
        }

        [Fact]
        public void CreateByteSpanOverParameter()
        {
            int test = ~0;
            var bytes = AsBytes(test);

            Assert.Equal(sizeof(int), bytes.Length);
            for (int i = 0; i < bytes.Length; i++)
            {
                byte currentByte = bytes[i];
                Assert.Equal(0xFF, currentByte);
            }
        }
    }
}
