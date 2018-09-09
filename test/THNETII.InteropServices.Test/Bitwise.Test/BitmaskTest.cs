using System;
using System.Linq;
using Xunit;

namespace THNETII.InteropServices.Bitwise.Test
{
    public abstract class BitmaskTest<T> where T : unmanaged
    {
        private static unsafe readonly int SizeOfT = sizeof(T);
        private static readonly int BitsInT = SizeOfT * 8;

        protected abstract T InverseDefault { get; }

        #region LowerBits
        protected abstract T LowerBits(int count);

        [Fact]
        public void LowerBitsWithNegativeCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                LowerBits(-1);
            });
        }

        [Fact]
        public void LowerBitsWithTooBigCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                LowerBits(BitsInT + 1);
            });
        }

        [Fact]
        public void LowerBitsZeroCountReturnsZero() =>
            Assert.Equal(default, LowerBits(count: 0));

        [Fact]
        public void LowerBitsWithCountNumberOfBitsSetsAllBits() =>
            Assert.Equal(InverseDefault, LowerBits(BitsInT));
        #endregion
        #region HigherBits
        protected abstract T HigherBits(int count);

        [Fact]
        public void HigherBitsWithNegativeCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                HigherBits(-1);
            });
        }

        [Fact]
        public void HigherBitsWithTooBigCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                HigherBits(BitsInT + 1);
            });
        }

        [Fact]
        public void HigherBitsZeroCountReturnsZero() =>
            Assert.Equal(default, HigherBits(count: 0));

        [Fact]
        public void HigherBitsWithCountNumberOfBitsSetsAllBits() =>
            Assert.Equal(InverseDefault, HigherBits(BitsInT));
        #endregion
        #region OffsetBits
        protected abstract T OffsetBits(int offset, int count);

        [Fact]
        public void OffsetBitsNegativeOffsetThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("offset", () =>
            {
                OffsetBits(-1, default);
            });
        }

        [Fact]
        public void OffsetBitsTooBigOffsetThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("offset", () =>
            {
                OffsetBits(BitsInT + 1, default);
            });
        }

        [Fact]
        public void OffsetBitsNegativeCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                OffsetBits(default, -1);
            });
        }

        [Fact]
        public void OffsetBitsTooBigCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                OffsetBits(default, BitsInT + 1);
            });
        }

        [Fact]
        public void OffsetBitsZeroZeroReturnsZero() =>
            Assert.Equal(default, OffsetBits(0, 0));

        [Fact]
        public void OffsetBitsSizeOfTime8OffsetReturnsZero() =>
            Assert.Equal(default, OffsetBits(BitsInT, default));

        [Fact]
        public void OffsetBitsZeroSizeOfTimes8SetsAllBits() =>
            Assert.Equal(InverseDefault, OffsetBits(default, BitsInT));
        #endregion
        #region OffsetRemaining
        protected abstract T OffsetRemaining(int offset);

        [Fact]
        public void OffsetRemainingWithNegativeOffsetThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("offset", () =>
            {
                OffsetRemaining(-1);
            });
        }

        [Fact]
        public void OffsetRemainingTooBigOffsetThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("offset", () =>
            {
                OffsetRemaining(BitsInT + 1);
            });
        }

        [Fact]
        public void OffsetRemainingZeroOffsetSetsAllBits() =>
            Assert.Equal(InverseDefault, OffsetRemaining(0));

        [Fact]
        public void OffsetRemainingSizeOfTimes8ReturnsZero() =>
            Assert.Equal(default, OffsetRemaining(BitsInT));
        #endregion
    }
}
