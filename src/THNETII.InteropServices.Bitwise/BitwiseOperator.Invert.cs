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
        /// Performs a bitwise inversion of the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the value to invert. Can be any primitive type that can be used in an unmanged context.</typeparam>
        /// <param name="value">A read-only reference of the value to invert.</param>
        /// <param name="result">Receives bitwise inverse of <paramref name="value"/>.</param>
        public static void Invert<T>(in T value, out T result) where T : unmanaged =>
            Unary(value, out result, PrimitiveInvert, PrimitiveInvert, PrimitiveInvert, PrimitiveInvert);

        /// <summary>
        /// Performs a bitwise inversion of the specified span of values.
        /// </summary>
        /// <param name="operand">A read-only span of the bytes to invert.</param>
        /// <param name="destination">A span to which the result of the operation is written. Must be equal in length to the <paramref name="operand"/>.</param>
        public static void Invert<T>(ReadOnlySpan<T> operand, Span<T> destination) where T : unmanaged =>
            Unary(operand, destination, PrimitiveInvert, PrimitiveInvert, PrimitiveInvert, PrimitiveInvert);

        /// <summary>
        /// Performs an in-place bitwise inversion of the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the value to invert. Can be any primitive type that can be used in an unmanged context.</typeparam>
        /// <param name="assignee">The value to invert, passed by reference.</param>
        public static void InvertInplace<T>(ref T assignee) where T : unmanaged =>
            UnaryInplace(ref assignee, PrimitiveInvert, PrimitiveInvert, PrimitiveInvert, PrimitiveInvert);
    }
}
