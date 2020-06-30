namespace THNETII.InteropServices.Bitwise.Test
{
    public class Bitfield32Test : BitfieldTest<uint>
    {
        protected override uint AllBitsSet { get; } = ~0U;
        protected override uint LowestBitSet { get; } = 1U;
        protected override uint AllExceptLowestBitSet { get; } = ~1U;
        protected override uint HighestBitSet { get; } = 1U << BitsInT;
        protected override uint AllExceptHighestBitSet { get; } = ~(1U << BitsInT);

        protected override IBitfield<uint> Bit(int index) =>
            Bitfield32.Bit(index);
        protected override IBitfield<uint> FromMask(uint mask, int shiftAmount) =>
            Bitfield32.FromMask(mask, shiftAmount);
        protected override IBitfield<uint> LowBits(int count) =>
            Bitfield32.LowBits(count);
        protected override IBitfield<uint> SelectBits(int offset, int count) =>
            Bitfield32.SelectBits(offset, count);
        protected override IBitfield<uint> RemainingBits(int offset) =>
            Bitfield32.RemainingBits(offset);
        protected override IBitfield<uint> HighBits(int offset) =>
            Bitfield32.HighBits(offset);
    }

    public class Bitfield32SignedTest : BitfieldTest<int>
    {
        protected override int AllBitsSet { get; } = ~0;
        protected override int LowestBitSet { get; } = 1;
        protected override int AllExceptLowestBitSet { get; } = ~1;
        protected override int HighestBitSet { get; } = 1 << BitsInT;
        protected override int AllExceptHighestBitSet { get; } = ~(1 << BitsInT);

        protected override IBitfield<int> Bit(int index) =>
            Bitfield32.Bit(index);
        protected override IBitfield<int> FromMask(int mask, int shiftAmount) =>
            Bitfield32.FromMask(mask, shiftAmount);
        protected override IBitfield<int> LowBits(int count) =>
            Bitfield32.LowBits(count);
        protected override IBitfield<int> SelectBits(int offset, int count) =>
            Bitfield32.SelectBits(offset, count);
        protected override IBitfield<int> RemainingBits(int offset) =>
            Bitfield32.RemainingBits(offset);
        protected override IBitfield<int> HighBits(int offset) =>
            Bitfield32.HighBits(offset);
    }
}
