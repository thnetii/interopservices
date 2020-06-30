namespace THNETII.InteropServices.Bitwise.Test
{
    public class Bitmask16BitsTest : BitmaskTest<ushort>
    {
        protected override ushort InverseDefault { get; } =
            unchecked((ushort)~0);

        protected override ushort LowerBits(int count) =>
            Bitmask.LowerBitsUInt16(count);

        protected override ushort HigherBits(int count) =>
            Bitmask.HigherBitsUInt16(count);

        protected override ushort OffsetBits(int offset, int count) =>
            Bitmask.OffsetBitsUInt16(offset, count);

        protected override ushort OffsetRemaining(int offset) =>
            Bitmask.OffsetRemainingUInt16(offset);
    }
}
