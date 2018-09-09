namespace THNETII.InteropServices.Bitwise.Test
{
    public class BitmaskSigned08BitsTest : BitmaskTest<sbyte>
    {
        protected override sbyte InverseDefault { get; } = ~0;

        protected override sbyte LowerBits(int count) =>
            Bitmask.LowerBitsInt8(count);

        protected override sbyte HigherBits(int count) =>
            Bitmask.HigherBitsInt8(count);

        protected override sbyte OffsetBits(int offset, int count) =>
            Bitmask.OffsetBitsInt8(offset, count);

        protected override sbyte OffsetRemaining(int offset) =>
            Bitmask.OffsetRemainingInt8(offset);
    }
}
