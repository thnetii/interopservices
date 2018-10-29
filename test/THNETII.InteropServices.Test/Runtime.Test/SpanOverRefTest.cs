using System;
using Xunit;

namespace THNETII.InteropServices.Runtime.Test
{
    public class SpanOverRefTest
    {
        private void AssertSpan(ref int variable, Span<int> span)
        {
            Assert.Equal(1, span.Length);
            Assert.Equal(variable, span[0]);
            span[0] = 24;
            Assert.Equal(24, variable);
        }

        private void AssertReadOnlySpan(ref int variable, ReadOnlySpan<int> span)
        {
            Assert.Equal(1, span.Length);
            Assert.Equal(variable, span[0]);
            variable = 24;
            Assert.Equal(24, span[0]);
        }

        [SkippableFact]
        public void GetSpanOverLocalVariableThrowsIfNotSupported()
        {
            Skip.If(SpanOverRef.IsCreateSpanSupported, $"{nameof(SpanOverRef)}.{nameof(SpanOverRef.IsCreateSpanSupported)} returned true");

            int test = 42;
            Assert.Throws<InvalidOperationException>(() => SpanOverRef.GetSpan(ref test));
        }

        [SkippableFact]
        public void GetReadOnlySpanOverLocalVariableThrowsIfNotSupported()
        {
            Skip.If(SpanOverRef.IsCreateSpanSupported, $"{nameof(SpanOverRef)}.{nameof(SpanOverRef.IsCreateSpanSupported)} returned true");

            int test = 42;
            Assert.Throws<InvalidOperationException>(() => SpanOverRef.GetReadOnlySpan(test));
        }

        [SkippableFact]
        public void GetSpanOverLocalVariable()
        {
            Skip.If(!SpanOverRef.IsCreateSpanSupported, $"{nameof(SpanOverRef)}.{nameof(SpanOverRef.IsCreateSpanSupported)} returned false");

            int test = 42;
            AssertSpan(ref test, SpanOverRef.GetSpan(ref test));
        }

        [SkippableFact]
        public void GetReadOnlySpanOverLocalVariable()
        {
            Skip.If(!SpanOverRef.IsCreateSpanSupported, $"{nameof(SpanOverRef)}.{nameof(SpanOverRef.IsCreateSpanSupported)} returned false");

            int test = 42;
            AssertReadOnlySpan(ref test, SpanOverRef.GetReadOnlySpan(test));
        }

        [SkippableFact]
        public void GetSpanOrCopyGetsSpanIfSupported()
        {
            Skip.If(!SpanOverRef.IsCreateSpanSupported, $"{nameof(SpanOverRef)}.{nameof(SpanOverRef.IsCreateSpanSupported)} returned false");
            int test = 42;
            AssertSpan(ref test, SpanOverRef.GetSpanOrCopy(ref test, out bool isCopy));
            Assert.False(isCopy);
        }

        [SkippableFact]
        public void GetSpanOrCopyCreatesCopyIfNotSupported()
        {
            Skip.If(SpanOverRef.IsCreateSpanSupported, $"{nameof(SpanOverRef)}.{nameof(SpanOverRef.IsCreateSpanSupported)} returned false");
            int test = 42;
            SpanOverRef.GetSpanOrCopy(ref test, out bool isCopy);
            Assert.True(isCopy);
        }

        [SkippableFact]
        public void GetReadOnlySpanOrCopyGetsSpanIfSupported()
        {
            Skip.If(!SpanOverRef.IsCreateSpanSupported, $"{nameof(SpanOverRef)}.{nameof(SpanOverRef.IsCreateSpanSupported)} returned false");
            int test = 42;
            AssertReadOnlySpan(ref test, SpanOverRef.GetReadOnlySpanOrCopy(test, out bool isCopy));
            Assert.False(isCopy);
        }

        [SkippableFact]
        public void GetReadOnlySpanOrCopyCreatesCopyIfNotSupported()
        {
            Skip.If(SpanOverRef.IsCreateSpanSupported, $"{nameof(SpanOverRef)}.{nameof(SpanOverRef.IsCreateSpanSupported)} returned false");
            int test = 42;
            SpanOverRef.GetReadOnlySpanOrCopy(test, out bool isCopy);
            Assert.True(isCopy);
        }
    }
}
