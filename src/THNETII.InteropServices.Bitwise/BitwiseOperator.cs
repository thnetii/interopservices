#if false
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using THNETII.InteropServices.Runtime;

namespace THNETII.InteropServices.Bitwise
{
    /// <summary>
    /// Supplies bitwise operators for binary blittable types of any size.
    /// </summary>
    [SuppressMessage("Usage", "PC001:API not supported on all platforms")]
    public static class BitwiseOperator
    {
        /// <summary>
        /// Returns the bitwise inverse of the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the value to invert. Can be any primitive type that can be used in an unmanged context.</typeparam>
        /// <param name="value">A read-only reference of the value to invert.</param>
        /// <returns>The bitwise invert of <paramref name="value"/>.</returns>
        public static T Invert<T>(in T value) where T : unmanaged
        {
            if (SpanOverRef.IsCreateSpanSupported)
            {
                var inSpan = SpanOverRef.CopyOrSpanReadOnly(value,
                        out bool isCopy, out Span<T> targetSpan);
                if (!isCopy)
                {
                    unsafe
                    {
                        Span<T> stackSpan = stackalloc T[1];
                        InvertUnguarded(
                            MemoryMarshal.AsBytes(stackSpan),
                            MemoryMarshal.AsBytes(inSpan));
                        return stackSpan[0];
                    }
                }
                else
                {
                    InvertUnguarded(
                        MemoryMarshal.AsBytes(targetSpan),
                        MemoryMarshal.AsBytes(inSpan));
                    return targetSpan[0];
                }
            }
            unsafe
            {
                Span<T> inSpan = stackalloc T[1];
                inSpan[0] = value;
                Span<T> stackSpan = stackalloc T[1];
                InvertUnguarded(
                    MemoryMarshal.AsBytes(stackSpan),
                    MemoryMarshal.AsBytes(inSpan));
                return stackSpan[0];
            }
        }

        /// <summary>
        /// Returns the bitwise inverse of the specified bytes.
        /// </summary>
        /// <param name="span">A read-only span of the bytes to invert.</param>
        /// <returns>The bitwise invert of <paramref name="span"/> as a heap-allocated array.</returns>
        public static byte[] Invert(ReadOnlySpan<byte> span)
        {
            if (span.IsEmpty)
                return Array.Empty<byte>();
            var destination = new byte[span.Length];
            InvertUnguarded(destination, span);
            return destination;
        }

        /// <summary>
        /// Writes the bitwise inverse of the specified input to the specified destination.
        /// </summary>
        /// <param name="destination">The destination to which to write the bitwise inverse.</param>
        /// <param name="input">The original bytes to invert.</param>
        /// <remarks>
        /// If <paramref name="destination"/> and <paramref name="input"/> refer to the same underlying memory location,
        /// the operation performs the bitwise inverse in-place.
        /// </remarks>
        public static void Invert(Span<byte> destination, ReadOnlySpan<byte> input)
        {
            if (destination.Length != input.Length)
            {
                throw new ArgumentException(
                    "The destination buffer must be equal in length to the specified input.",
                    paramName: nameof(destination));
            }
            InvertUnguarded(destination, input);
        }

        private static void InvertUnguarded(Span<byte> destination, ReadOnlySpan<byte> input)
        {
            switchStart:
            switch (input.Length)
            {
                default: // > sizeof(ulong)
                case sizeof(ulong):
                    {
                        ulong inverse = ~MemoryMarshal.Read<ulong>(input);
                        MemoryMarshal.Write(destination, ref inverse);

                        const int sliceStart = sizeof(ulong);
                        input = input.Slice(sliceStart);
                        destination = destination.Slice(sliceStart);
                    }
                    goto switchStart;
                case sizeof(uint) + 3:
                case sizeof(uint) + 2:
                case sizeof(uint) + 1:
                case sizeof(uint):
                    {
                        uint inverse = ~MemoryMarshal.Read<uint>(input);
                        MemoryMarshal.Write(destination, ref inverse);

                        const int sliceStart = sizeof(uint);
                        input = input.Slice(sliceStart);
                        destination = destination.Slice(sliceStart);
                    }
                    goto switchStart;
                case sizeof(ushort) + 1:
                case sizeof(ushort):
                    {
                        ushort inverse = unchecked((ushort)
                            ~(uint)MemoryMarshal.Read<ushort>(input));
                        MemoryMarshal.Write(destination, ref inverse);

                        const int sliceStart = sizeof(ushort);
                        input = input.Slice(sliceStart);
                        destination = destination.Slice(sliceStart);
                    }
                    goto switchStart;
                case sizeof(byte):
                    {
                        uint value = input[0];
                        destination[0] = unchecked((byte)~value);

                        const int sliceStart = sizeof(byte);
                        input = input.Slice(sliceStart);
                        destination = destination.Slice(sliceStart);
                    }
                    goto switchStart;
                case 0:
                    return;
            }
        }

        private static uint PrimitiveAnd(uint left, uint right) => left & right;
        private static ulong PrimitiveAnd(ulong left, ulong right) => left & right;

        public static T And<T>(in T left, in T right) where T : unmanaged =>
            PrimitiveBinary(left, right, PrimitiveAnd, PrimitiveAnd);

        public static byte[] And(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right) =>
            PrimitiveBinary(left, right, PrimitiveAnd, PrimitiveAnd);

        public static void AndInplace(Span<byte> assignee, ReadOnlySpan<byte> operand) =>
            PrimitiveBinaryInplace(assignee, operand, PrimitiveAnd, PrimitiveAnd);

        private static uint PrimitiveOr(uint left, uint right) => left | right;
        private static ulong PrimitiveOr(ulong left, ulong right) => left | right;

        public static T Or<T>(in T left, in T right) where T : unmanaged =>
            PrimitiveBinary(left, right, PrimitiveOr, PrimitiveOr);

        public static byte[] Or(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right) =>
            PrimitiveBinary(left, right, PrimitiveOr, PrimitiveOr);

        public static void OrInplace(Span<byte> assignee, ReadOnlySpan<byte> operand) =>
            PrimitiveBinaryInplace(assignee, operand, PrimitiveOr, PrimitiveOr);

        private static uint PrimitiveXor(uint left, uint right) => left ^ right;
        private static ulong PrimitiveXor(ulong left, ulong right) => left ^ right;

        public static T Xor<T>(in T left, in T right) where T : unmanaged =>
            PrimitiveBinary(left, right, PrimitiveXor, PrimitiveXor);

        public static byte[] Xor(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right) =>
            PrimitiveBinary(left, right, PrimitiveXor, PrimitiveXor);

        public static void XorInplace(Span<byte> assignee, ReadOnlySpan<byte> operand) =>
            PrimitiveBinaryInplace(assignee, operand, PrimitiveXor, PrimitiveXor);

        public static T PrimitiveBinary<T>(in T left, in T right,
            Func<uint, uint, uint> op32Bits, Func<ulong, ulong, ulong> op64Bits)
            where T : unmanaged
        {
            if (op32Bits == null)
                throw new ArgumentNullException(nameof(op32Bits));
            else if (op64Bits == null)
                throw new ArgumentNullException(nameof(op64Bits));
            unsafe
            {
                Span<T> lSpan = stackalloc T[1];
                lSpan[0] = left;
                Span<T> rSpan = stackalloc T[1];
                rSpan[0] = right;
                var lBytes = MemoryMarshal.AsBytes(lSpan);
                var rBytes = MemoryMarshal.AsBytes(rSpan);
                PrimitiveBinaryUnguarded(lBytes, lBytes, rBytes, op32Bits, op64Bits);
                return lSpan[0];
            }
        }

        public static byte[] PrimitiveBinary(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right,
            Func<uint, uint, uint> op32Bits, Func<ulong, ulong, ulong> op64Bits)
        {
            if (left.Length != right.Length)
            {
                throw new ArgumentException(
                    message: "The operands passed as input must be equal in length.",
                    paramName: nameof(right)
                    );
            }
            if (op32Bits == null)
                throw new ArgumentNullException(nameof(op32Bits));
            else if (op64Bits == null)
                throw new ArgumentNullException(nameof(op64Bits));
            int length = left.Length;
            if (length == 0)
                return Array.Empty<byte>();
            var destination = new byte[length];
            PrimitiveBinaryUnguarded(destination, left, right, op32Bits, op64Bits);
            return destination;
        }

        public static void PrimitiveBinaryInplace(Span<byte> assignee, ReadOnlySpan<byte> operand,
            Func<uint, uint, uint> op32Bits, Func<ulong, ulong, ulong> op64Bits)
        {
            if (assignee.Length != operand.Length)
            {
                throw new ArgumentException(
                    message: $"The {nameof(operand)} must be equal in length to the {nameof(assignee)}.",
                    paramName: nameof(operand)
                    );
            }
            if (op32Bits == null)
                throw new ArgumentNullException(nameof(op32Bits));
            else if (op64Bits == null)
                throw new ArgumentNullException(nameof(op64Bits));
            PrimitiveBinaryUnguarded(assignee, assignee, operand,
                op32Bits, op64Bits);
        }

        private static void PrimitiveBinaryUnguarded(Span<byte> destination,
            ReadOnlySpan<byte> left, ReadOnlySpan<byte> right,
            Func<uint, uint, uint> op32Bits, Func<ulong, ulong, ulong> op64Bits)
        {
            switchStart:
            switch (destination.Length)
            {
                default: // > sizeof(ulong)
                case sizeof(ulong):
                    {
                        ulong opResult = op64Bits(
                            MemoryMarshal.Read<ulong>(left),
                            MemoryMarshal.Read<ulong>(right)
                            );
                        MemoryMarshal.Write(destination, ref opResult);

                        const int sliceStart = sizeof(ulong);
                        left = left.Slice(start: sliceStart);
                        right = right.Slice(start: sliceStart);
                        destination = destination.Slice(start: sliceStart);
                    }
                    goto switchStart;
                case sizeof(uint) + 3:
                case sizeof(uint) + 2:
                case sizeof(uint) + 1:
                case sizeof(uint):
                    {
                        uint opResult = op32Bits(
                            MemoryMarshal.Read<uint>(left),
                            MemoryMarshal.Read<uint>(right)
                            );
                        MemoryMarshal.Write(destination, ref opResult);

                        const int sliceStart = sizeof(uint);
                        left = left.Slice(start: sliceStart);
                        right = right.Slice(start: sliceStart);
                        destination = destination.Slice(start: sliceStart);
                    }
                    goto switchStart;
                case sizeof(ushort) + 1:
                case sizeof(ushort):
                    {
                        ushort opResult = unchecked((ushort)op32Bits(
                            MemoryMarshal.Read<ushort>(left),
                            MemoryMarshal.Read<ushort>(right)
                            ));
                        MemoryMarshal.Write(destination, ref opResult);

                        const int sliceStart = sizeof(ushort);
                        left = left.Slice(start: sliceStart);
                        right = right.Slice(start: sliceStart);
                        destination = destination.Slice(start: sliceStart);
                    }
                    goto switchStart;
                case sizeof(byte):
                    {
                        destination[0] = unchecked((byte)op32Bits(
                            left[0], right[0]
                            ));

                        const int sliceStart = sizeof(byte);
                        left = left.Slice(start: sliceStart);
                        right = right.Slice(start: sliceStart);
                        destination = destination.Slice(start: sliceStart);
                    }
                    goto switchStart;
                case 0:
                    return;
            }
        }
    } 
}
#endif
