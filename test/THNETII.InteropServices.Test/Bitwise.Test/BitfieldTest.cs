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

        protected abstract IBitfield<T> DefineSingleBit(int index);
        protected abstract IBitfield<T> DefineFromMask(T mask, int shiftAmount);
        protected abstract IBitfield<T> DefineLowerBits(int count);
        protected abstract IBitfield<T> DefineMiddleBits(int offset, int count);
        protected abstract IBitfield<T> DefineRemainingBits(int offset);
        protected abstract IBitfield<T> DefineHigherBits(int offset);

        [Fact]
        public void ReadLowestBitFromAllBitsSetReturnsOne()
        {
            var def = DefineSingleBit(0);
            var storage = AllBitsSet;
            Assert.Equal(LowestBitSet, def.Read(storage));
        }

        [Fact]
        public void ReadHighestBitFromAllBitsSetReturnsOne()
        {
            var def = DefineSingleBit(BitsInT - 1);
            var storage = AllBitsSet;
            Assert.Equal(LowestBitSet, def.Read(storage));
        }

        [Fact]
        public void ReadLowestBitFromZeroReturnsZero()
        {
            var def = DefineSingleBit(0);
            var storage = NoBitsSet;
            Assert.Equal(NoBitsSet, def.Read(storage));
        }

        [Fact]
        public void ReadLowestBitFromAllSetExceptLowestReturnsZero()
        {
            var def = DefineSingleBit(0);
            var storage = AllExceptLowestBitSet;
            Assert.Equal(NoBitsSet, def.Read(storage));
        }

        [Fact]
        public void ReadHighestBitFromZeroReturnsZero()
        {
            var def = DefineSingleBit(0);
            var storage = NoBitsSet;
            Assert.Equal(NoBitsSet, def.Read(storage));
        }

        [Fact]
        public void ReadHighestBitFromAllSetExceptHighestReturnsZero()
        {
            var def = DefineSingleBit(0);
            var storage = AllExceptHighestBitSet;
            Assert.Equal(NoBitsSet, def.Read(storage));
        }

        [Fact]
        public void DefineSingleBitWithNegativeIndexThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("index", () =>
            {
                DefineSingleBit(-1);
            });
        }

        [Fact]
        public void DefineSingleBitWithTooBigIndexThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("index", () =>
            {
                DefineSingleBit(BitsInT);
            });
        }

        [Fact]
        public void DefineSingleBit0HasCorrectMasks()
        {
            var def = DefineSingleBit(0);
            Assert.Equal(LowestBitSet, def.Mask);
            Assert.Equal(AllExceptLowestBitSet, def.InverseMask);
        }

        [Fact]
        public void DefineSingleBit0HasZeroShiftAmount()
        {
            var def = DefineSingleBit(0);
            Assert.Equal(0, def.ShiftAmount);
        }

        [Fact]
        public void DefineFromMaskWithNegativeShiftAmountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("shiftAmount", () =>
            {
                DefineFromMask(default, -1);
            });
        }

        [Fact]
        public void DefineFromMaskWithTooBigShiftAmountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("shiftAmount", () =>
            {
                DefineFromMask(default, BitsInT + 1);
            });
        }

        [Fact]
        public void DefineFromMaskNoBitsHasAllBitsInverseMask()
        {
            var def = DefineFromMask(NoBitsSet, default);
            Assert.Equal(AllBitsSet, def.InverseMask);
        }

        [Fact]
        public void DefineLowerBitsWithNegativeCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                DefineLowerBits(-1);
            });
        }

        [Fact]
        public void DefineLowerBitsWithTooBigCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                DefineLowerBits(BitsInT + 1);
            });
        }

        [Fact]
        public void DefineHigherBitsWithNegativeCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                DefineHigherBits(-1);
            });
        }

        [Fact]
        public void DefineHigherBitsWithTooBigCountThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () =>
            {
                DefineHigherBits(BitsInT + 1);
            });
        }
    }
}
