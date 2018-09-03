using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.Bitwise
{
    [SuppressMessage("Usage", "PC001:API not supported on all platforms")]
    public static class BitwiseOperator
    {
        public static unsafe T Invert<T>(in T value) where T : unmanaged
        {
            Span<T> span = stackalloc T[1];
            span[0] = value;
            InvertInplace(MemoryMarshal.AsBytes(span));
            return span[0];
        }

        public static byte[] Invert(ReadOnlySpan<byte> span)
        {
            if (span.IsEmpty)
                return Array.Empty<byte>();
            var destination = new byte[span.Length];
            span.CopyTo(destination);
            InvertInplace(destination);
            return destination;
        }

        public static void InvertInplace(Span<byte> bytes)
        {
            if (bytes.Length > sizeof(ulong))
            {
                InvertInplace(bytes.Slice(start: 0, length: sizeof(ulong)));
                InvertInplace(bytes.Slice(start: sizeof(ulong)));
            }
            else if (bytes.Length == sizeof(ulong))
            {
                ulong inverse = ~MemoryMarshal.Read<ulong>(bytes);
                MemoryMarshal.Write(bytes, ref inverse);
            }
            else if (bytes.Length > sizeof(uint))
            {
                InvertInplace(bytes.Slice(start: 0, length: sizeof(uint)));
                InvertInplace(bytes.Slice(start: sizeof(uint)));
            }
            else if (bytes.Length == sizeof(uint))
            {
                var inverse = ~MemoryMarshal.Read<uint>(bytes);
                MemoryMarshal.Write(bytes, ref inverse);
            }
            else if (bytes.Length > sizeof(ushort))
            {
                InvertInplace(bytes.Slice(start: 0, length: sizeof(ushort)));
                InvertInplace(bytes.Slice(start: sizeof(ushort)));
            }
            else if (bytes.Length == sizeof(ushort))
            {
                uint actual = MemoryMarshal.Read<ushort>(bytes);
                ushort inverse = unchecked((ushort)~actual);
                MemoryMarshal.Write(bytes, ref inverse);
            }
            else if (bytes.Length == sizeof(byte))
            {
                uint actual = bytes[0];
                bytes[0] = unchecked((byte)~actual);
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

        public static unsafe T PrimitiveBinary<T>(in T left, in T right,
            Func<uint, uint, uint> op32Bits, Func<ulong, ulong, ulong> op64Bits)
            where T : unmanaged
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

        public static byte[] PrimitiveBinary(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right,
            Func<uint, uint, uint> op32Bits, Func<ulong, ulong, ulong> op64Bits)
        {
            if (left.Length != right.Length)
            {
                throw new ArgumentException(
                    message: "The length of the byte-spans passed as operands must be equal in length.",
                    paramName: nameof(right)
                    );
            }
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
                    message: "The length of the byte-spans passed as operands must be equal in length.",
                    paramName: nameof(operand)
                    );
            }
            PrimitiveBinaryUnguarded(assignee, assignee, operand,
                op32Bits, op64Bits);
        }

        private static void PrimitiveBinaryUnguarded(Span<byte> destination,
            ReadOnlySpan<byte> left, ReadOnlySpan<byte> right,
            Func<uint, uint, uint> op32Bits, Func<ulong, ulong, ulong> op64Bits)
        {
            if (destination.Length > sizeof(ulong))
            {
                PrimitiveBinaryUnguarded(destination.Slice(0, length: sizeof(ulong)),
                    left.Slice(0, length: sizeof(ulong)),
                    right.Slice(0, length: sizeof(ulong)),
                    op32Bits, op64Bits
                    );
                PrimitiveBinaryUnguarded(destination.Slice(start: sizeof(ulong)),
                    left.Slice(start: sizeof(ulong)),
                    right.Slice(start: sizeof(ulong)),
                    op32Bits, op64Bits
                    );
            }
            else if (destination.Length == sizeof(ulong))
            {
                ulong lValue, rValue, andValue;
                lValue = MemoryMarshal.Read<ulong>(left);
                rValue = MemoryMarshal.Read<ulong>(right);
                andValue = op64Bits(lValue, rValue);
                MemoryMarshal.Write(destination, ref andValue);
            }
            else if (destination.Length > sizeof(uint))
            {
                PrimitiveBinaryUnguarded(destination.Slice(0, length: sizeof(uint)),
                    left.Slice(0, length: sizeof(uint)),
                    right.Slice(0, length: sizeof(uint)),
                    op32Bits, op64Bits
                    );
                PrimitiveBinaryUnguarded(destination.Slice(start: sizeof(uint)),
                    left.Slice(start: sizeof(uint)),
                    right.Slice(start: sizeof(uint)),
                    op32Bits, op64Bits
                    );
            }
            else if (destination.Length == sizeof(uint))
            {
                uint lValue, rValue, andValue;
                lValue = MemoryMarshal.Read<uint>(left);
                rValue = MemoryMarshal.Read<uint>(right);
                andValue = op32Bits(lValue, rValue);
                MemoryMarshal.Write(destination, ref andValue);
            }
            else if (destination.Length > sizeof(ushort))
            {
                PrimitiveBinaryUnguarded(destination.Slice(0, length: sizeof(ushort)),
                    left.Slice(0, length: sizeof(ushort)),
                    right.Slice(0, length: sizeof(ushort)),
                    op32Bits, op64Bits
                    );
                PrimitiveBinaryUnguarded(destination.Slice(start: sizeof(ushort)),
                    left.Slice(start: sizeof(ushort)),
                    right.Slice(start: sizeof(ushort)),
                    op32Bits, op64Bits
                    );
            }
            else if (destination.Length == sizeof(ushort))
            {
                uint lValue, rValue;
                lValue = MemoryMarshal.Read<ushort>(left);
                rValue = MemoryMarshal.Read<ushort>(right);
                ushort andValue = unchecked((ushort)op32Bits(lValue, rValue));
                MemoryMarshal.Write(destination, ref andValue);
            }
            else if (destination.Length == sizeof(byte))
            {
                uint lValue, rValue;
                lValue = left[0];
                rValue = right[0];
                destination[0] = unchecked((byte)op32Bits(lValue, rValue));
            }
        }
    }
}
