namespace THNETII.InteropServices.Bitwise.Test
{
    public class BitmaskSigned16BitsTest : BitmaskTest<short>
    {
        protected override short InverseDefault { get; } = ~0;

        protected override short LowerBits(int count) =>
            Bitmask.LowerBitsInt16(count);

        protected override short HigherBits(int count) =>
            Bitmask.HigherBitsInt16(count);

        protected override short OffsetBits(int offset, int count) =>
            Bitmask.OffsetBitsInt16(offset, count);

        protected override short OffsetRemaining(int offset) =>
            Bitmask.OffsetRemainingInt16(offset);
    }
}
