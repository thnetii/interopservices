namespace THNETII.InteropServices.Bitwise.Test
{
    public class Bitfield32Test : BitfieldTest<uint>
    {
        protected override uint AllBitsSet { get; } = ~0U;
        protected override uint LowestBitSet { get; } = 1U;
        protected override uint AllExceptLowestBitSet { get; } = ~1U;
        protected override uint HighestBitSet { get; } = 1U << BitsInT;
        protected override uint AllExceptHighestBitSet { get; } = ~(1U << BitsInT);

        protected override IBitfield<uint> DefineSingleBit(int index) =>
            Bitfield32.DefineSingleBit(index);
        protected override IBitfield<uint> DefineFromMask(uint mask, int shiftAmount) =>
            Bitfield32.DefineFromMask(mask, shiftAmount);
        protected override IBitfield<uint> DefineLowerBits(int count) =>
            Bitfield32.DefineLowerBits(count);
        protected override IBitfield<uint> DefineMiddleBits(int offset, int count) =>
            Bitfield32.DefineMiddleBits(offset, count);
        protected override IBitfield<uint> DefineRemainingBits(int offset) =>
            Bitfield32.DefineRemainingBits(offset);
        protected override IBitfield<uint> DefineHigherBits(int offset) =>
            Bitfield32.DefineHigherBits(offset);
    }

    public class Bitfield32SignedTest : BitfieldTest<int>
    {
        protected override int AllBitsSet { get; } = ~0;
        protected override int LowestBitSet { get; } = 1;
        protected override int AllExceptLowestBitSet { get; } = ~1;
        protected override int HighestBitSet { get; } = 1 << BitsInT;
        protected override int AllExceptHighestBitSet { get; } = ~(1 << BitsInT);

        protected override IBitfield<int> DefineSingleBit(int index) =>
            Bitfield32.DefineSingleBit(index);
        protected override IBitfield<int> DefineFromMask(int mask, int shiftAmount) =>
            Bitfield32.DefineFromMask(mask, shiftAmount);
        protected override IBitfield<int> DefineLowerBits(int count) =>
            Bitfield32.DefineLowerBits(count);
        protected override IBitfield<int> DefineMiddleBits(int offset, int count) =>
            Bitfield32.DefineMiddleBits(offset, count);
        protected override IBitfield<int> DefineRemainingBits(int offset) =>
            Bitfield32.DefineRemainingBits(offset);
        protected override IBitfield<int> DefineHigherBits(int offset) =>
            Bitfield32.DefineHigherBits(offset);
    }
}
