using System;
using Xunit;

namespace THNETII.InteropServices.Memory.Test
{
    public static class FixedTest
    {
        [Fact]
        public static unsafe void UsePointerRefersToSameValueAsReference()
        {
            int variable = 42;
            Fixed.Use(ref variable, ptr =>
            {
                int* intPtr = (int*)ptr;
                Assert.Equal(variable, *intPtr);
                variable = 24;
                Assert.Equal(variable, *intPtr);
            });
        }

        private struct MyManagedStruct
        {
            public byte[] bytes;
        }

        [Fact]
        public static unsafe void UseCanUseNonUnmanagedTypeArgument()
        {
            var s = new MyManagedStruct();
            Fixed.Use(ref s, ptr =>
            {
                Assert.False(ptr == null);
            });
        }

        [Fact]
        public static void UseWithNullActionThrows()
        {
            int variable = 42;
            Assert.ThrowsAny<ArgumentNullException>(() => Fixed.Use(ref variable, null));
        }

        [Fact]
        public static void UseWithNullFuncThrows()
        {
            int variable = 42;
            Assert.ThrowsAny<ArgumentNullException>(() => Fixed.Use<int, int>(ref variable, null));
        }
    }
}
