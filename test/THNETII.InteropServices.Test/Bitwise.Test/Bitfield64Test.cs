namespace THNETII.InteropServices.Bitwise.Test
{
    public class Bitfield64Test : BitfieldTest<ulong>
    {
        protected override ulong AllBitsSet { get; } = ~0UL;
        protected override ulong LowestBitSet { get; } = 1UL;
        protected override ulong AllExceptLowestBitSet { get; } = ~1UL;
        protected override ulong HighestBitSet { get; } = 1UL << BitsInT;
        protected override ulong AllExceptHighestBitSet { get; } = ~(1UL << BitsInT);

        protected override IBitfield<ulong> Bit(int index) =>
            Bitfield64.Bit(index);
        protected override IBitfield<ulong> FromMask(ulong mask, int shiftAmount) =>
            Bitfield64.FromMask(mask, shiftAmount);
        protected override IBitfield<ulong> LowBits(int count) =>
            Bitfield64.LowBits(count);
        protected override IBitfield<ulong> SelectBits(int offset, int count) =>
            Bitfield64.SelectBits(offset, count);
        protected override IBitfield<ulong> RemainingBits(int offset) =>
            Bitfield64.RemainingBits(offset);
        protected override IBitfield<ulong> HighBits(int offset) =>
            Bitfield64.HighBits(offset);
    }

    public class Bitfield64SignedTest : BitfieldTest<long>
    {
        protected override long AllBitsSet { get; } = ~0L;
        protected override long LowestBitSet { get; } = 1L;
        protected override long AllExceptLowestBitSet { get; } = ~1L;
        protected override long HighestBitSet { get; } = 1L << BitsInT;
        protected override long AllExceptHighestBitSet { get; } = ~(1L << BitsInT);

        protected override IBitfield<long> Bit(int index) =>
            Bitfield64.Bit(index);
        protected override IBitfield<long> FromMask(long mask, int shiftAmount) =>
            Bitfield64.FromMask(mask, shiftAmount);
        protected override IBitfield<long> LowBits(int count) =>
            Bitfield64.LowBits(count);
        protected override IBitfield<long> SelectBits(int offset, int count) =>
            Bitfield64.SelectBits(offset, count);
        protected override IBitfield<long> RemainingBits(int offset) =>
            Bitfield64.RemainingBits(offset);
        protected override IBitfield<long> HighBits(int offset) =>
            Bitfield64.HighBits(offset);
    }
}
