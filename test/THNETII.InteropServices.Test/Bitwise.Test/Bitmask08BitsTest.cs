namespace THNETII.InteropServices.Bitwise.Test
{
    public class Bitmask08BitsTest : BitmaskTest<byte>
    {
        protected override byte InverseDefault { get; } =
            unchecked((byte)~0);

        protected override byte LowerBits(int count) =>
            Bitmask.LowerBitsUInt8(count);

        protected override byte HigherBits(int count) =>
            Bitmask.HigherBitsUInt8(count);

        protected override byte OffsetBits(int offset, int count) =>
            Bitmask.OffsetBitsUInt8(offset, count);

        protected override byte OffsetRemaining(int offset) =>
            Bitmask.OffsetRemainingUInt8(offset);
    }
}
