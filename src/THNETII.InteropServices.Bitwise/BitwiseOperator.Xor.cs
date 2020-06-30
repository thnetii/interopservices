using System;

namespace THNETII.InteropServices.Bitwise
{
    partial class BitwiseOperator
    {
        private static byte PrimitiveXor(byte left, byte right) => (byte)((uint)left ^ right);
        private static ushort PrimitiveXor(ushort left, ushort right) => (ushort)((uint)left ^ right);
        private static uint PrimitiveXor(uint left, uint right) => left ^ right;
        private static ulong PrimitiveXor(ulong left, ulong right) => left ^ right;

        /// <summary>
        /// Performs an bitwise exclusive OR (XOR) operation of the first two operands and
        /// stores the result in the third.
        /// </summary>
        /// <typeparam name="T">The unmanaged blittable type of the operands.</typeparam>
        /// <param name="left">The left operand, passed as a readonly reference.</param>
        /// <param name="right">The right operand, passed as a readonly reference.</param>
        /// <param name="result">Receives the result of the bitwise exclusive OR (XOR) between <paramref name="left"/> and <paramref name="right"/>.</param>
        /// <remarks>
        /// Since a bitwise exclusive OR (XOR) operation is performed on the individual bits of
        /// the operands, independently of their posisition, the operation can
        /// be performed on any blittable type, regardless of size, layout or
        /// endianness.
        /// <para>
        /// The operation is performed by taking the span of the operands and
        /// re-interpreting the underlying bits as primitive numeric types
        /// (<see cref="ulong"/>, <see cref="uint"/>, <see cref="ushort"/> and
        /// <see cref="byte"/> in that order). The algorithm uses the native
        /// bitwise operation for the largest possible type and iterates until
        /// no more bytes are remaining.
        /// </para>
        /// </remarks>
        public static void Xor<T>(in T left, in T right, out T result) where T : unmanaged =>
            Binary(left, right, out result, PrimitiveXor, PrimitiveXor, PrimitiveXor, PrimitiveXor);

        /// <summary>
        /// Performs an bitwise exclusive OR (XOR) operation of the first two operands and
        /// stores the result in the third.
        /// </summary>
        /// <typeparam name="T">The unmanaged blittable type of the operands.</typeparam>
        /// <param name="left">A span forming the left operand.</param>
        /// <param name="right">A span forming the right operand. Must be equal in length to <paramref name="left"/>.</param>
        /// <param name="destination">A span to which the result of the operation will be written. Must be equal in length to both the <paramref name="left"/> and <paramref name="right"/> operands.</param>
        /// <remarks>
        /// Since a bitwise exclusive OR (XOR) operation is performed on the individual bits of
        /// the operands, independently of their posisition, the operation can
        /// be performed on any blittable type, regardless of size, layout or
        /// endianness.
        /// <para>
        /// The operation is performed by taking the span of the operands and
        /// re-interpreting the underlying bits as primitive numeric types
        /// (<see cref="ulong"/>, <see cref="uint"/>, <see cref="ushort"/> and
        /// <see cref="byte"/> in that order). The algorithm uses the native
        /// bitwise operation for the largest possible type and iterates until
        /// no more bytes are remaining.
        /// </para>
        /// </remarks>
        public static void Xor<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right, Span<T> destination) where T : unmanaged =>
            Binary(left, right, destination, PrimitiveXor, PrimitiveXor, PrimitiveXor, PrimitiveXor);

        /// <summary>
        /// Performs an bitwise exclusive OR (XOR) operation of the first two operands and
        /// overwrites the first operand with the result.
        /// </summary>
        /// <typeparam name="T">The unmanaged blittable type of the operands.</typeparam>
        /// <param name="assignee">The left operand, passed as a readonly reference which is overwritten with the result of the operation.</param>
        /// <param name="operand">The right operand, passed as a readonly reference.</param>
        /// <remarks>
        /// Since a bitwise exclusive OR (XOR) operation is performed on the individual bits of
        /// the operands, independently of their posisition, the operation can
        /// be performed on any blittable type, regardless of size, layout or
        /// endianness.
        /// <para>
        /// The operation is performed by taking the span of the operands and
        /// re-interpreting the underlying bits as primitive numeric types
        /// (<see cref="ulong"/>, <see cref="uint"/>, <see cref="ushort"/> and
        /// <see cref="byte"/> in that order). The algorithm uses the native
        /// bitwise operation for the largest possible type and iterates until
        /// no more bytes are remaining.
        /// </para>
        /// </remarks>
        public static void XorInplace<T>(ref T assignee, in T operand) where T : unmanaged =>
            BinaryInplace(ref assignee, operand, PrimitiveXor, PrimitiveXor, PrimitiveXor, PrimitiveXor);
    }
}
