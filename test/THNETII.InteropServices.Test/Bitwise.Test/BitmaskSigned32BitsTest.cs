namespace THNETII.InteropServices.Bitwise.Test
{
    public class BitmaskSigned32BitsTest : BitmaskTest<int>
    {
        protected override int InverseDefault { get; } = ~0;

        protected override int LowerBits(int count) =>
            Bitmask.LowerBitsInt32(count);

        protected override int HigherBits(int count) =>
            Bitmask.HigherBitsInt32(count);

        protected override int OffsetBits(int offset, int count) =>
            Bitmask.OffsetBitsInt32(offset, count);

        protected override int OffsetRemaining(int offset) =>
            Bitmask.OffsetRemainingInt32(offset);
    }
}
