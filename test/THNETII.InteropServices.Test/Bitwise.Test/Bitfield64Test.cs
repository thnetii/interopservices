namespace THNETII.InteropServices.Bitwise.Test
{
    public class Bitfield64Test : BitfieldTest<ulong>
    {
        protected override ulong AllBitsSet { get; } = ~0UL;
        protected override ulong LowestBitSet { get; } = 1UL;
        protected override ulong AllExceptLowestBitSet { get; } = ~1UL;
        protected override ulong HighestBitSet { get; } = 1UL << BitsInT;
        protected override ulong AllExceptHighestBitSet { get; } = ~(1UL << BitsInT);

        protected override IBitfield<ulong> DefineSingleBit(int index) =>
            Bitfield64.DefineSingleBit(index);
        protected override IBitfield<ulong> DefineFromMask(ulong mask, int shiftAmount) =>
            Bitfield64.DefineFromMask(mask, shiftAmount);
        protected override IBitfield<ulong> DefineLowerBits(int count) =>
            Bitfield64.DefineLowerBits(count);
        protected override IBitfield<ulong> DefineMiddleBits(int offset, int count) =>
            Bitfield64.DefineMiddleBits(offset, count);
        protected override IBitfield<ulong> DefineRemainingBits(int offset) =>
            Bitfield64.DefineRemainingBits(offset);
        protected override IBitfield<ulong> DefineHigherBits(int offset) =>
            Bitfield64.DefineHigherBits(offset);
    }

    public class Bitfield64SignedTest : BitfieldTest<long>
    {
        protected override long AllBitsSet { get; } = ~0L;
        protected override long LowestBitSet { get; } = 1L;
        protected override long AllExceptLowestBitSet { get; } = ~1L;
        protected override long HighestBitSet { get; } = 1L << BitsInT;
        protected override long AllExceptHighestBitSet { get; } = ~(1L << BitsInT);

        protected override IBitfield<long> DefineSingleBit(int index) =>
            Bitfield64.DefineSingleBit(index);
        protected override IBitfield<long> DefineFromMask(long mask, int shiftAmount) =>
            Bitfield64.DefineFromMask(mask, shiftAmount);
        protected override IBitfield<long> DefineLowerBits(int count) =>
            Bitfield64.DefineLowerBits(count);
        protected override IBitfield<long> DefineMiddleBits(int offset, int count) =>
            Bitfield64.DefineMiddleBits(offset, count);
        protected override IBitfield<long> DefineRemainingBits(int offset) =>
            Bitfield64.DefineRemainingBits(offset);
        protected override IBitfield<long> DefineHigherBits(int offset) =>
            Bitfield64.DefineHigherBits(offset);
    }
}
