namespace THNETII.InteropServices.Bitwise.Test
{
    public class Bitmask64BitsTest : BitmaskTest<ulong>
    {
        protected override ulong InverseDefault { get; } = ~0UL;

        protected override ulong LowerBits(int count) =>
            Bitmask.LowerBitsUInt64(count);

        protected override ulong HigherBits(int count) =>
            Bitmask.HigherBitsUInt64(count);

        protected override ulong OffsetBits(int offset, int count) =>
            Bitmask.OffsetBitsUInt64(offset, count);

        protected override ulong OffsetRemaining(int offset) =>
            Bitmask.OffsetRemainingUInt64(offset);
    }
}
