using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.Bitwise
{
    partial class BitwiseOperator
    {
        private static unsafe void Unary<T>(in T operand, out T result,
            Func<uint, uint> op32Bits, Func<ulong, ulong> op64Bits,
            Func<ushort, ushort> op16Bits, Func<byte, byte> op8Bits)
            where T : unmanaged
        {
            fixed (T* opPtr = &operand)
            fixed (T* rPtr = &result)
            {
                ReadOnlySpan<T> opSpan = new ReadOnlySpan<T>(opPtr, 1);
                Span<T> resultSpan = new Span<T>(rPtr, 1);
                UnaryUnguarded(opSpan, resultSpan,
                    op32Bits, op64Bits, op16Bits, op8Bits);
            }
        }

        [SuppressMessage("Globalization", "CA1303: Do not pass literals as localized parameters")]
        private static void Unary<T>(ReadOnlySpan<T> operand, Span<T> destination,
            Func<uint, uint> op32Bits, Func<ulong, ulong> op64Bits,
            Func<ushort, ushort> op16Bits, Func<byte, byte> op8Bits)
            where T : unmanaged
        {
            if (destination.Length != operand.Length)
                throw new ArgumentException("Destination length must be equal to the length of the operand", nameof(destination));

            UnaryUnguarded(operand, destination,
                op32Bits, op64Bits, op16Bits, op8Bits);
        }

        private static unsafe void UnaryInplace<T>(ref T assignee,
            Func<uint, uint> op32Bits, Func<ulong, ulong> op64Bits,
            Func<ushort, ushort> op16Bits, Func<byte, byte> op8Bits)
            where T : unmanaged
        {
            fixed (T* aPtr = &assignee)
            {
                Span<T> aSpan = new Span<T>(aPtr, 1);
                UnaryUnguarded(aSpan, aSpan,
                    op32Bits, op64Bits, op16Bits, op8Bits);
            }
        }

        private static void UnaryUnguarded<T>(ReadOnlySpan<T> operand, Span<T> result,
            Func<uint, uint> op32Bits, Func<ulong, ulong> op64Bits,
            Func<ushort, ushort> op16Bits, Func<byte, byte> op8Bits)
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
                PrimitiveUnaryUnguarded(ref byteOffset, operand, result,
                    op64Bits, ref bytesRemaining); 
            }

            // 32-Bits
            if (bytesRemaining == 0)
                return;
            else if (bytesRemaining >= sizeof(uint))
            {
                PrimitiveUnaryUnguarded(ref byteOffset, operand, result,
                    op32Bits, ref bytesRemaining); 
            }

            // 16-Bits
            if (bytesRemaining == 0)
                return;
            else if (bytesRemaining >= sizeof(ushort))
            {
                PrimitiveUnaryUnguarded(ref byteOffset, operand, result,
                    op16Bits, ref bytesRemaining); 
            }

            // 8-Bits
            if (bytesRemaining == 0)
                return;
            else if (bytesRemaining >= sizeof(byte))
            {
                PrimitiveUnaryUnguarded(ref byteOffset, operand, result,
                    op8Bits, ref bytesRemaining); 
            }
        }

        private static void PrimitiveUnaryUnguarded<T, TPrimitive>(ref int byteOffset,
            ReadOnlySpan<T> operand, Span<T> destination,
            Func<TPrimitive, TPrimitive> unary, ref int bytesRemaining)
            where T : struct
            where TPrimitive : unmanaged
        {
            int sizeofPrimitive;
            unsafe { sizeofPrimitive = sizeof(TPrimitive); }
            ReadOnlySpan<TPrimitive> opPrimitive = MemoryMarshal.Cast<T, TPrimitive>(operand);
            Span<TPrimitive> rPrimitive = MemoryMarshal.Cast<T, TPrimitive>(destination);
            int idx = byteOffset / sizeofPrimitive;
            bytesRemaining -= (rPrimitive.Length - idx) * sizeofPrimitive;
            for (; idx < rPrimitive.Length; idx++)
                rPrimitive[idx] = unary(opPrimitive[idx]);
            byteOffset = idx * sizeofPrimitive;
        }
    }
}
