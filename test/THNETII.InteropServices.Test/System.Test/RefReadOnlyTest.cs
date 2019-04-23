using System;

using Xunit;

namespace System.Test
{
    public static class RefReadOnlyTest
    {
        [Fact]
        public static void FixedPointerOfRefReadonlyOnStackDoesNotCopy()
        {
            int t = 24;

            AssertNoChange(in t, () => { t = 42; });

            Assert.Equal(42, t);

            unsafe void AssertNoChange(in int value, Action changeValue)
            {
                fixed(int* p1 = &value)
                {
                    int i1 = *p1;
                    changeValue();
                    int i2 = *p1;

                    Assert.NotEqual(i1, i2);
                }
            }
        }
    }
}
