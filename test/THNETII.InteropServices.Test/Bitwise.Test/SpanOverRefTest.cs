using System;
using System.Linq;
using Xunit;

namespace THNETII.InteropServices.Bitwise.Test
{
    public class SpanOverRefTest
    {
        [Fact]
        public void CreateSpanOverLocalVariable()
        {
            int test = 42;
            var span = SpanOverRef.CopyOrSpan(ref test, out bool isCopy);
            Assert.False(span.IsEmpty);
            Assert.Equal(1, span.Length);
            Assert.Equal(test, span[0]);
            Assert.Equal(!SpanOverRef.IsCreateSpanSupported, isCopy);
            if (isCopy)
            {
                span[0] += 1;
                Assert.NotEqual(test, span[0]);
            }
            else
            {
                span[0] += 1;
                Assert.Equal(test, span[0]);
                test = 24;
                Assert.Equal(test, span[0]);
            }
        }

        [Fact]
        public void CreateReadOnlySpanOverLocalVariable()
        {
            int test = 42;
            var span = SpanOverRef.CopyOrSpanReadOnly(test, out bool isCopy);
            Assert.False(span.IsEmpty);
            Assert.Equal(1, span.Length);
            Assert.Equal(test, span[0]);
            Assert.Equal(!SpanOverRef.IsCreateSpanSupported, isCopy);
            if (isCopy)
            {
                test += 1;
                Assert.NotEqual(test, span[0]);
            }
            else
            {
                test = 24;
                Assert.Equal(test, span[0]);
            }
        }
    }
}
