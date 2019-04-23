using System;

namespace THNETII.InteropServices.Bitwise
{
    partial class BitwiseOperator
    {
        private static byte PrimitiveAnd(byte left, byte right) => (byte)((uint)left & right);
        private static ushort PrimitiveAnd(ushort left, ushort right) => (ushort)((uint)left & right);
        private static uint PrimitiveAnd(uint left, uint right) => left & right;
        private static ulong PrimitiveAnd(ulong left, ulong right) => left & right;

        public static void And<T>(in T left, in T right, out T result)
            where T : unmanaged =>
            Binary(left, right, out result, PrimitiveAnd, PrimitiveAnd, PrimitiveAnd, PrimitiveAnd);

        public static void And<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right, Span<T> destination)
            where T : unmanaged =>
            Binary(left, right, destination, PrimitiveAnd, PrimitiveAnd, PrimitiveAnd, PrimitiveAnd);

        public static void AndInplace<T>(ref T assignee, in T operand)
            where T : unmanaged =>
            BinaryInplace(ref assignee, operand, PrimitiveAnd, PrimitiveAnd, PrimitiveAnd, PrimitiveAnd);
    }
}
