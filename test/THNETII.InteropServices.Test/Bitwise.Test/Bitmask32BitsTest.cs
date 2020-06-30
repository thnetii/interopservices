namespace THNETII.InteropServices.Bitwise.Test
{
    public class Bitmask32BitsTest : BitmaskTest<uint>
    {
        protected override uint InverseDefault { get; } = ~0U;

        protected override uint LowerBits(int count) =>
            Bitmask.LowerBitsUInt32(count);

        protected override uint HigherBits(int count) =>
            Bitmask.HigherBitsUInt32(count);

        protected override uint OffsetBits(int offset, int count) =>
            Bitmask.OffsetBitsUInt32(offset, count);

        protected override uint OffsetRemaining(int offset) =>
            Bitmask.OffsetRemainingUInt32(offset);
    }
}
