using System;
using System.Runtime.InteropServices;
using Xunit;

namespace THNETII.InteropServices.Memory.Test
{
    public static class SpanOverRefTest
    {
        private static void AssertSpan(ref int variable, Span<int> span)
        {
            Assert.Equal(1, span.Length);
            Assert.Equal(variable, span[0]);
            span[0] = 24;
            Assert.Equal(24, variable);
        }

        private static void AssertReadOnlySpan(ref int variable, ReadOnlySpan<int> span)
        {
            Assert.Equal(1, span.Length);
            Assert.Equal(variable, span[0]);
            variable = 24;
            Assert.Equal(24, span[0]);
        }

        [SkippableFact]
        public static void GetSpanOverLocalVariable()
        {
            int test = 42;
            AssertSpan(ref test, SpanOverRef.GetSpan(ref test));
        }

        [SkippableFact]
        public static void GetReadOnlySpanOverLocalVariable()
        {
            int test = 42;
            AssertReadOnlySpan(ref test, SpanOverRef.GetReadOnlySpan(test));
        }

        [StructLayout(LayoutKind.Sequential)]
        private class HeapClass
        {
            public int field;
            public Span<int> GetFieldSpan(out GCHandle pinnedHandle) => SpanOverRef.GetPinnedSpan(ref field, this, out pinnedHandle);
            public ReadOnlySpan<int> GetReadOnlyFieldSpan(out GCHandle pinnedHandle) => SpanOverRef.GetPinnedReadOnlySpan(field, this, out pinnedHandle);
        }

        [Fact]
        public static void GetPinnedSpanOverHeapVariable()
        {
            var test = new HeapClass() { field = 42 };
            var span = test.GetFieldSpan(out var pinnedHandle);
            try
            {
                AssertSpan(ref test.field, span);
            }
            finally
            {
                if (pinnedHandle.IsAllocated)
                    pinnedHandle.Free();
            }
        }

        [Fact]
        public static void GetPinnedReadOnlySpanOverHeapVariable()
        {
            var test = new HeapClass() { field = 42 };
            var span = test.GetReadOnlyFieldSpan(out var pinnedHandle);
            try
            {
                AssertReadOnlySpan(ref test.field, span);
            }
            finally
            {
                if (pinnedHandle.IsAllocated)
                    pinnedHandle.Free();
            }
        }
    }
}
