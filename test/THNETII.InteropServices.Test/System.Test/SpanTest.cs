using Xunit;

namespace System.Test
{
    public class SpanTest
    {
        [Fact]
        public unsafe void StackAllocSpanLength()
        {
            const int count = 5;
            Span<int> span = stackalloc int[count];

            Assert.Equal(count, span.Length);
        }
    }
}
