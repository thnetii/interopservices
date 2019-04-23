using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THNETII.InteropServices.Bitwise
{
    partial class BitwiseOperator
    {
        private static byte PrimitiveOr(byte left, byte right) => (byte)((uint)left | right);
        private static ushort PrimitiveOr(ushort left, ushort right) => (ushort)((uint)left | right);
        private static uint PrimitiveOr(uint left, uint right) => left | right;
        private static ulong PrimitiveOr(ulong left, ulong right) => left | right;

        public static void Or<T>(in T left, in T right, out T result) where T : unmanaged =>
            Binary(left, right, out result, PrimitiveOr, PrimitiveOr, PrimitiveOr, PrimitiveOr);

        public static void Or<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right, Span<T> destination) where T : unmanaged =>
            Binary(left, right, destination, PrimitiveOr, PrimitiveOr, PrimitiveOr, PrimitiveOr);

        public static void OrInplace<T>(ref T assignee, in T operand) where T : unmanaged =>
            BinaryInplace(ref assignee, operand, PrimitiveOr, PrimitiveOr, PrimitiveOr, PrimitiveOr);
    }
}
