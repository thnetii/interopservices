using System;

namespace THNETII.InteropServices.Bitwise
{
    partial class BitwiseOperator
    {
        private static byte PrimitiveXor(byte left, byte right) => (byte)((uint)left ^ right);
        private static ushort PrimitiveXor(ushort left, ushort right) => (ushort)((uint)left ^ right);
        private static uint PrimitiveXor(uint left, uint right) => left ^ right;
        private static ulong PrimitiveXor(ulong left, ulong right) => left ^ right;

        public static void Xor<T>(in T left, in T right, out T result) where T : unmanaged =>
            Binary(left, right, out result, PrimitiveXor, PrimitiveXor, PrimitiveXor, PrimitiveXor);

        public static void Xor<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right, Span<T> destination) where T : unmanaged =>
            Binary(left, right, destination, PrimitiveXor, PrimitiveXor, PrimitiveXor, PrimitiveXor);

        public static void XorInplace<T>(ref T assignee, in T operand) where T : unmanaged =>
            BinaryInplace(ref assignee, operand, PrimitiveXor, PrimitiveXor, PrimitiveXor, PrimitiveXor);
    }
}
