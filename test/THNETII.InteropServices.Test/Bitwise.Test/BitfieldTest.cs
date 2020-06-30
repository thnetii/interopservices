using System;
using Xunit;

namespace THNETII.InteropServices.Bitwise.Test
{
    public abstract class BitfieldTest<T> where T : unmanaged
    {
        protected static unsafe readonly int SizeOfT = sizeof(T);
        protected static readonly int BitsInT = SizeOfT * 8;

        protected virtual T NoBitsSet { get; } = default;
        protected abstract T AllBitsSet { get; }
        protected abstract T LowestBitSet { get; }
        protected abstract T AllExceptLowestBitSet { get; }
        protected abstract T HighestBitSet { get; }
        protected abstract T AllExceptHighestBitSet { get; }

        protected abstract IBitfield<T> Bit(int index);
        protected abstract IBitfield<T> FromMask(T mask, int shiftAmount);
        protected abstract IBitfield<T> LowBits(int count);
        protected abstract IBitfield<T> SelectBits(int offset, int count);
        protected abstract IBitfield<T> RemainingBits(int offset);
        protected abstract IBitfield<T> HighBits(int offset);

        [Fact]
        public void ReadLowestBitFromAllBitsSetReturnsOne()
        {
            var def = Bit(0);
            var storage = AllBitsSet;
            Assert.Equal(LowestBitSet, def.Read(storage));
        }

        [Fact]
        public void ReadHighestBitFromAllBitsSetReturnsOne()
        {
            var def = Bit(BitsInT - 1);
            var storage = AllBitsSet;
            Assert.Equal(LowestBitSet, def.Read(storage));
        }

        [Fact]
        public void ReadLowestBitFromZeroReturnsZero()
        {
            var def = Bit(0);
            var storage = NoBitsSet;
            Assert.Equal(NoBitsSet, def.Read(storage));
        }

        [Fact]
        public void ReadLowestBitFromAllSetExceptLowestReturnsZero()
        {
            var def = Bit(0);
            var storage = AllExceptLowestBitSet;
            Assert.Equal(NoBitsSet, def.Read(storage));
        }

        [Fact]
        public void ReadHighestBitFromZeroReturnsZero()
        {
            var def = Bit(0);
            var storage = NoBitsSet;
            Assert.Equal(NoBitsSet, def.Read(storage));
        }

        [Fact]
        public void ReadHighestBitFromAllSetExceptHighestReturnsZero()
        {
            var def = Bit(0);
            var storage = AllExceptHighestBitSet;
            Assert.Equal(NoBitsSet, def.Read(storage));
        }

        [Fact]
        public void DefineSingleBitWithNegativeIndexThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("index", () =>
            {
                Bit(-1);
            });
        }

        [Fact]
        public void DefineSingleBitWithTooBigIndexThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("index", () =>
            {
                Bit(BitsInT);
            });
        }

        [Fact]
        public void DefineSingleBit0HasCorrectMasks()
        {
            var def = Bit(0);
            Assert.Equal(LowestBitSet, def.Mask);
            Assert.Equal(AllExceptLowestBitSet, def.InverseMask);
        }

        [Fact]
        public void DefineSingleBit0HasZeroShiftAmount()
        {
            var def = Bit(0);
            Assert.Equal(0, def.ShiftAmount);
        }

        [Fact]
        public void DefineFromMaskWithNegativeShiftAmountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("shiftAmount", () =>
            {
                FromMask(default, -1);
            });
        }

        [Fact]
        public void DefineFromMaskWithTooBigShiftAmountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("shiftAmount", () =>
            {
                FromMask(default, BitsInT + 1);
            });
        }

        [Fact]
        public void DefineFromMaskNoBitsHasAllBitsInverseMask()
        {
            var def = FromMask(NoBitsSet, default);
            Assert.Equal(AllBitsSet, def.InverseMask);
        }

        [Fact]
        public void DefineLowerBitsWithNegativeCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                LowBits(-1);
            });
        }

        [Fact]
        public void DefineLowerBitsWithTooBigCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                LowBits(BitsInT + 1);
            });
        }

        [Fact]
        public void DefineHigherBitsWithNegativeCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                HighBits(-1);
            });
        }

        [Fact]
        public void DefineHigherBitsWithTooBigCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                HighBits(BitsInT + 1);
            });
        }
    }
}
