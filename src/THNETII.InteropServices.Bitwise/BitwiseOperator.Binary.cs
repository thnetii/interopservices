using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.Bitwise
{
    partial class BitwiseOperator
    {
        private static void Binary<T>(in T left, in T right, out T result,
            Func<uint, uint, uint> op32Bits, Func<ulong, ulong, ulong> op64Bits,
            Func<ushort, ushort, ushort> op16Bits, Func<byte, byte, byte> op8Bits)
            where T : unmanaged
        {
            unsafe
            {
                fixed (T* lPtr = &left)
                fixed (T* rPtr = &right)
                fixed (T* resultPtr = &result)
                {
                    ReadOnlySpan<T> lSpan = new ReadOnlySpan<T>(lPtr, 1);
                    ReadOnlySpan<T> rSpan = new ReadOnlySpan<T>(rPtr, 1);
                    Span<T> resultSpan = new Span<T>(resultPtr, 1);

                    BinaryUnguarded(lSpan, rSpan, resultSpan,
                        op32Bits, op64Bits, op16Bits, op8Bits);
                }
            }
        }

        [SuppressMessage("Globalization", "CA1303: Do not pass literals as localized parameters")]
        private static void Binary<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right, Span<T> destination,
            Func<uint, uint, uint> op32Bits, Func<ulong, ulong, ulong> op64Bits,
            Func<ushort, ushort, ushort> op16Bits, Func<byte, byte, byte> op8Bits)
            where T : unmanaged
        {
            if (right.Length != left.Length)
                throw new ArgumentException("Length of operands must be equal.", nameof(right));
            else if (destination.Length != left.Length)
                throw new ArgumentException("Destination length must be equal to the length of the operands", nameof(destination));
            BinaryUnguarded(left, right, destination,
                op32Bits, op64Bits, op16Bits, op8Bits);
        }

        private static void BinaryInplace<T>(ref T assignee, in T operand,
            Func<uint, uint, uint> op32Bits, Func<ulong, ulong, ulong> op64Bits,
            Func<ushort, ushort, ushort> op16Bits, Func<byte, byte, byte> op8Bits)
            where T : unmanaged
        {
            unsafe
            {
                fixed (T* aPtr = &assignee)
                fixed (T* opPtr = &operand)
                {
                    Span<T> aSpan = new Span<T>(aPtr, 1);
                    ReadOnlySpan<T> opSpan = new ReadOnlySpan<T>(opPtr, 1);

                    BinaryUnguarded(aSpan, opSpan, aSpan,
                        op32Bits, op64Bits, op16Bits, op8Bits);
                }
            }
        }

        private static void BinaryUnguarded<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right, Span<T> result,
            Func<uint, uint, uint> op32Bits, Func<ulong, ulong, ulong> op64Bits,
            Func<ushort, ushort, ushort> op16Bits, Func<byte, byte, byte> op8Bits)
            where T : unmanaged
        {
            int byteOffset = 0;
            int bytesRemaining;
            unsafe { bytesRemaining = result.Length * sizeof(T); }

            // 64-Bits
            if (bytesRemaining == 0)
                return;
            else if (bytesRemaining >= sizeof(ulong))
            {
                PrimitiveBinaryUnguarded(ref byteOffset, left, right, result,
                    op64Bits, ref bytesRemaining);
            }

            // 32-Bits
            if (bytesRemaining == 0)
                return;
            else if (bytesRemaining >= sizeof(uint))
            {
                PrimitiveBinaryUnguarded(ref byteOffset, left, right, result,
                    op32Bits, ref bytesRemaining);
            }

            // 16-Bits
            if (bytesRemaining == 0)
                return;
            else if (bytesRemaining >= sizeof(ushort))
            {
                PrimitiveBinaryUnguarded(ref byteOffset, left, right, result,
                    op16Bits, ref bytesRemaining);
            }

            // 8-Bits
            if (bytesRemaining == 0)
                return;
            else if (bytesRemaining >= sizeof(byte))
            {
                PrimitiveBinaryUnguarded(ref byteOffset, left, right, result,
                    op8Bits, ref bytesRemaining);
            }
        }

        private static void PrimitiveBinaryUnguarded<T, TPrimitive>(ref int byteOffset,
            ReadOnlySpan<T> left, ReadOnlySpan<T> right, Span<T> destination,
            Func<TPrimitive, TPrimitive, TPrimitive> op, ref int bytesRemaining)
            where T : struct
            where TPrimitive : unmanaged
        {
            int sizeofPrimitive;
            unsafe { sizeofPrimitive = sizeof(TPrimitive); }
            ReadOnlySpan<TPrimitive> lPrimitive = MemoryMarshal.Cast<T, TPrimitive>(left);
            ReadOnlySpan<TPrimitive> rPrimitive = MemoryMarshal.Cast<T, TPrimitive>(right);
            Span<TPrimitive> dPrimitive = MemoryMarshal.Cast<T, TPrimitive>(destination);
            int idx = byteOffset / sizeofPrimitive;
            bytesRemaining -= (dPrimitive.Length - idx) * sizeofPrimitive;
            for (; idx < dPrimitive.Length; idx++)
                dPrimitive[idx] = op(lPrimitive[idx], rPrimitive[idx]);
            byteOffset = idx * sizeofPrimitive;
        }
    }
}
