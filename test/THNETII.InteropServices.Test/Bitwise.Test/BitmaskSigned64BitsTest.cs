namespace THNETII.InteropServices.Bitwise.Test
{
    public class BitmaskSigned64BitsTest : BitmaskTest<long>
    {
        protected override long InverseDefault { get; } = ~0L;

        protected override long LowerBits(int count) =>
            Bitmask.LowerBitsInt64(count);

        protected override long HigherBits(int count) =>
            Bitmask.HigherBitsInt64(count);

        protected override long OffsetBits(int offset, int count) =>
            Bitmask.OffsetBitsInt64(offset, count);

        protected override long OffsetRemaining(int offset) =>
            Bitmask.OffsetRemainingInt64(offset);
    }
}
