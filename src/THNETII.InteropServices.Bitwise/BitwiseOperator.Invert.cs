using System;

namespace THNETII.InteropServices.Bitwise
{
    partial class BitwiseOperator
    {
        private static byte PrimitiveInvert(byte op) => (byte)~(uint)op;
        private static ushort PrimitiveInvert(ushort op) => (ushort)~(uint)op;
        private static uint PrimitiveInvert(uint op) => ~op;
        private static ulong PrimitiveInvert(ulong op) => ~op;

        /// <summary>
        /// Returns the bitwise inverse of the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the value to invert. Can be any primitive type that can be used in an unmanged context.</typeparam>
        /// <param name="value">A read-only reference of the value to invert.</param>
        /// <returns>The bitwise invert of <paramref name="value"/>.</returns>
        public static void Invert<T>(in T value, out T result) where T : unmanaged =>
            Unary(value, out result, PrimitiveInvert, PrimitiveInvert, PrimitiveInvert, PrimitiveInvert);

        /// <summary>
        /// Returns the bitwise inverse of the specified bytes.
        /// </summary>
        /// <param name="operand">A read-only span of the bytes to invert.</param>
        /// <returns>The bitwise invert of <paramref name="operand"/> as a heap-allocated array.</returns>
        public static void Invert<T>(ReadOnlySpan<T> operand, Span<T> destination) where T : unmanaged =>
            Unary(operand, destination, PrimitiveInvert, PrimitiveInvert, PrimitiveInvert, PrimitiveInvert);

        public static void InvertInplace<T>(ref T assignee) where T : unmanaged =>
            UnaryInplace(ref assignee, PrimitiveInvert, PrimitiveInvert, PrimitiveInvert, PrimitiveInvert);
    }
}
