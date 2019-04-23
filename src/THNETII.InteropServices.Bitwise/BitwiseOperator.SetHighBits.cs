namespace THNETII.InteropServices.Bitwise
{
    partial class BitwiseOperator
    {
        internal static byte PrimitiveSetHighBits8(int count) => (byte)(~0U << count);
        internal static ushort PrimitiveSetHighBits16(int count) => (ushort)(~0U << count);
        internal static uint PrimitiveSetHighBits32(int count) => ~0U << count;
        internal static ulong PrimitiveSetHighBits64(int count) => ~0UL << count;
    }
}
