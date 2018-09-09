using System;

namespace THNETII.InteropServices.Bitwise
{
    /// <summary>
    /// Priovides methods to create bitmasks with sequential bit patterns.
    /// </summary>
    public static class Bitmask
    {
        internal static class ThrowHelpers
        {
            internal static void ThrowForCount(int count, int byteSize, int offset = 0)
            {
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(count), actualValue: count,
                        "Number of bits must be a non-negative integer."
                        );
                }
                int bits = byteSize * 8 - offset;
                if (count > bits)
                {
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(count), actualValue: count,
                        $"Number of bits must not exceed the maximum number of available bits ({bits} bits)."
                        );
                }
            }

            internal static void ThrowForOffset(int offset, int byteSize)
            {
                if (offset < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(offset), actualValue: offset,
                        "Bit offset must be a non-negative integer."
                        );
                }
                int bits = byteSize * 8;
                if (offset > bits)
                {
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(offset), actualValue: offset,
                        $"Bit offset must be less than the number of available bits ({bits} bits)."
                        );
                }
            }
        }

        private static sbyte OffsetBitsInt8Unguarded(int offset, int count) =>
            unchecked((sbyte)OffsetBitsInt32Unguarded(offset, count));

        private static short OffsetBitsInt16Unguarded(int offset, int count) =>
            unchecked((short)OffsetBitsInt32Unguarded(offset, count));

        private static int OffsetBitsInt32Unguarded(int offset, int count)
        {
            int v = 0;
            for (int i = 0, bit = 1 << offset; i < count; i++, bit <<= 1)
                v |= bit;
            return v;
        }

        private static long OffsetBitsInt64Unguarded(int offset, int count)
        {
            long v = 0;
            int i = 0;
            for (long bit = 1L << offset; i < count; i++, bit <<= 1)
                v |= bit;
            return v;
        }

        private static byte OffsetBitsUInt8Unguarded(int offset, int count) =>
            unchecked((byte)OffsetBitsInt32Unguarded(offset, count));

        private static ushort OffsetBitsUInt16Unguarded(int offset, int count) =>
            unchecked((ushort)OffsetBitsInt32Unguarded(offset, count));

        private static uint OffsetBitsUInt32Unguarded(int offset, int count) =>
            unchecked((uint)OffsetBitsInt32Unguarded(offset, count));

        private static ulong OffsetBitsUInt64Unguarded(int offset, int count) =>
            unchecked((ulong)OffsetBitsInt64Unguarded(offset, count));

        /// <summary>
        /// Returns a <see cref="byte"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="byte"/> value.</param>
        /// <returns>A <see cref="byte"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="byte"/> value.</exception>
        public static byte LowerBitsUInt8(int count)
        {
            ThrowHelpers.ThrowForCount(count, sizeof(byte));
            return OffsetBitsUInt8Unguarded(offset: 0, count);
        }

        /// <summary>
        /// Returns a <see cref="ushort"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="ushort"/> value.</param>
        /// <returns>A <see cref="ushort"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="ushort"/> value.</exception>
        public static ushort LowerBitsUInt16(int count)
        {
            ThrowHelpers.ThrowForCount(count, sizeof(ushort));
            return OffsetBitsUInt16Unguarded(offset: 0, count);
        }

        /// <summary>
        /// Returns a <see cref="uint"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="uint"/> value.</param>
        /// <returns>A <see cref="uint"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="uint"/> value.</exception>
        public static uint LowerBitsUInt32(int count)
        {
            ThrowHelpers.ThrowForCount(count, sizeof(uint));
            return OffsetBitsUInt32Unguarded(offset: 0, count);
        }

        /// <summary>
        /// Returns a <see cref="ulong"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="ulong"/> value.</param>
        /// <returns>A <see cref="ulong"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="ulong"/> value.</exception>
        public static ulong LowerBitsUInt64(int count)
        {
            ThrowHelpers.ThrowForCount(count, sizeof(ulong));
            return OffsetBitsUInt64Unguarded(offset: 0, count);
        }

        /// <summary>
        /// Returns a <see cref="sbyte"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="sbyte"/> value.</param>
        /// <returns>A <see cref="sbyte"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="sbyte"/> value.</exception>
        public static sbyte LowerBitsInt8(int count)
        {
            ThrowHelpers.ThrowForCount(count, sizeof(sbyte));
            return OffsetBitsInt8Unguarded(offset: 0, count);
        }

        /// <summary>
        /// Returns a <see cref="short"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="short"/> value.</param>
        /// <returns>A <see cref="short"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="short"/> value.</exception>
        public static short LowerBitsInt16(int count)
        {
            ThrowHelpers.ThrowForCount(count, sizeof(short));
            return OffsetBitsInt16Unguarded(offset: 0, count);
        }

        /// <summary>
        /// Returns a <see cref="int"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="int"/> value.</param>
        /// <returns>A <see cref="int"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="int"/> value.</exception>
        public static int LowerBitsInt32(int count)
        {
            ThrowHelpers.ThrowForCount(count, sizeof(int));
            return OffsetBitsInt32Unguarded(offset: 0, count);
        }

        /// <summary>
        /// Returns a <see cref="long"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="long"/> value.</param>
        /// <returns>A <see cref="long"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="long"/> value.</exception>
        public static long LowerBitsInt64(int count)
        {
            ThrowHelpers.ThrowForCount(count, sizeof(long));
            return OffsetBitsInt64Unguarded(offset: 0, count);
        }

        /// <summary>
        /// Returns a <see cref="byte"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="byte"/> value.</param>
        /// <returns>A <see cref="byte"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="byte"/> value.</exception>
        public static byte HigherBitsUInt8(int count)
        {
            const int bytes = sizeof(byte);
            ThrowHelpers.ThrowForCount(count, bytes);
            const int bits = bytes * 8;
            return OffsetBitsUInt8Unguarded(offset: bits - count, count);
        }

        /// <summary>
        /// Returns a <see cref="ushort"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="ushort"/> value.</param>
        /// <returns>A <see cref="ushort"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="ushort"/> value.</exception>
        public static ushort HigherBitsUInt16(int count)
        {
            const int bytes = sizeof(ushort);
            ThrowHelpers.ThrowForCount(count, bytes);
            const int bits = bytes * 8;
            return OffsetBitsUInt16Unguarded(offset: bits - count, count);
        }

        /// <summary>
        /// Returns a <see cref="uint"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="uint"/> value.</param>
        /// <returns>A <see cref="uint"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="uint"/> value.</exception>
        public static uint HigherBitsUInt32(int count)
        {
            const int bytes = sizeof(uint);
            ThrowHelpers.ThrowForCount(count, bytes);
            const int bits = bytes * 8;
            return OffsetBitsUInt32Unguarded(offset: bits - count, count);
        }

        /// <summary>
        /// Returns a <see cref="ulong"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="ulong"/> value.</param>
        /// <returns>A <see cref="ulong"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="ulong"/> value.</exception>
        public static ulong HigherBitsUInt64(int count)
        {
            const int bytes = sizeof(ulong);
            ThrowHelpers.ThrowForCount(count, bytes);
            const int bits = bytes * 8;
            return OffsetBitsUInt64Unguarded(offset: bits - count, count);
        }

        /// <summary>
        /// Returns a <see cref="sbyte"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="sbyte"/> value.</param>
        /// <returns>A <see cref="sbyte"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="sbyte"/> value.</exception>
        public static sbyte HigherBitsInt8(int count)
        {
            const int bytes = sizeof(sbyte);
            ThrowHelpers.ThrowForCount(count, bytes);
            const int bits = bytes * 8;
            return OffsetBitsInt8Unguarded(offset: bits - count, count);
        }

        /// <summary>
        /// Returns a <see cref="short"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="short"/> value.</param>
        /// <returns>A <see cref="short"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="short"/> value.</exception>
        public static short HigherBitsInt16(int count)
        {
            const int bytes = sizeof(short);
            ThrowHelpers.ThrowForCount(count, bytes);
            const int bits = bytes * 8;
            return OffsetBitsInt16Unguarded(offset: bits - count, count);
        }

        /// <summary>
        /// Returns a <see cref="int"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="int"/> value.</param>
        /// <returns>A <see cref="int"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="int"/> value.</exception>
        public static int HigherBitsInt32(int count)
        {
            const int bytes = sizeof(int);
            ThrowHelpers.ThrowForCount(count, bytes);
            const int bits = bytes * 8;
            return OffsetBitsInt32Unguarded(offset: bits - count, count);
        }

        /// <summary>
        /// Returns a <see cref="long"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="long"/> value.</param>
        /// <returns>A <see cref="long"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="long"/> value.</exception>
        public static long HigherBitsInt64(int count)
        {
            const int bytes = sizeof(long);
            ThrowHelpers.ThrowForCount(count, bytes);
            const int bits = bytes * 8;
            return OffsetBitsInt64Unguarded(offset: bits - count, count);
        }

        /// <summary>
        /// Returns a <see cref="byte"/> value where the specified number of bits
        /// starting from the specified low-end bit offset are set. All remaining
        /// bits are unset.
        /// </summary>
        /// <param name="offset">
        /// The number of bits from the low-end at which to start setting bits.
        /// Must not exceed the number of available bits in a <see cref="byte"/> value.
        /// </param>
        /// <param name="count">
        /// The number of bits to set.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="byte"/> value with <paramref name="count"/> bits set,
        /// starting with the lowest bits set at the 0-based index indicated by
        /// <paramref name="offset"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number of
        /// bits in a <see cref="long"/> value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is negative or greater than the difference
        /// between the number of bits in a <see cref="long"/> value and
        /// <paramref name="offset"/>.
        /// </exception>
        public static byte OffsetBitsUInt8(int offset, int count)
        {
            ThrowHelpers.ThrowForOffset(offset, sizeof(byte));
            ThrowHelpers.ThrowForCount(count, sizeof(byte), offset);
            return OffsetBitsUInt8Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="ushort"/> value where the specified number of bits
        /// starting from the specified low-end bit offset are set. All remaining
        /// bits are unset.
        /// </summary>
        /// <param name="offset">
        /// The number of bits from the low-end at which to start setting bits.
        /// Must not exceed the number of available bits in a <see cref="ushort"/> value.
        /// </param>
        /// <param name="count">
        /// The number of bits to set.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="ushort"/> value with <paramref name="count"/> bits set,
        /// starting with the lowest bits set at the 0-based index indicated by
        /// <paramref name="offset"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number of
        /// bits in a <see cref="ushort"/> value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is negative or greater than the difference
        /// between the number of bits in a <see cref="ushort"/> value and
        /// <paramref name="offset"/>.
        /// </exception>
        public static ushort OffsetBitsUInt16(int offset, int count)
        {
            ThrowHelpers.ThrowForOffset(offset, sizeof(ushort));
            ThrowHelpers.ThrowForCount(count, sizeof(ushort), offset);
            return OffsetBitsUInt16Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="uint"/> value where the specified number of bits
        /// starting from the specified low-end bit offset are set. All remaining
        /// bits are unset.
        /// </summary>
        /// <param name="offset">
        /// The number of bits from the low-end at which to start setting bits.
        /// Must not exceed the number of available bits in a <see cref="uint"/> value.
        /// </param>
        /// <param name="count">
        /// The number of bits to set.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="uint"/> value with <paramref name="count"/> bits set,
        /// starting with the lowest bits set at the 0-based index indicated by
        /// <paramref name="offset"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number of
        /// bits in a <see cref="uint"/> value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is negative or greater than the difference
        /// between the number of bits in a <see cref="uint"/> value and
        /// <paramref name="offset"/>.
        /// </exception>
        public static uint OffsetBitsUInt32(int offset, int count)
        {
            ThrowHelpers.ThrowForOffset(offset, sizeof(uint));
            ThrowHelpers.ThrowForCount(count, sizeof(uint), offset);
            return OffsetBitsUInt32Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="ulong"/> value where the specified number of bits
        /// starting from the specified low-end bit offset are set. All remaining
        /// bits are unset.
        /// </summary>
        /// <param name="offset">
        /// The number of bits from the low-end at which to start setting bits.
        /// Must not exceed the number of available bits in a <see cref="ulong"/> value.
        /// </param>
        /// <param name="count">
        /// The number of bits to set.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="ulong"/> value with <paramref name="count"/> bits set,
        /// starting with the lowest bits set at the 0-based index indicated by
        /// <paramref name="offset"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number of
        /// bits in a <see cref="ulong"/> value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is negative or greater than the difference
        /// between the number of bits in a <see cref="ulong"/> value and
        /// <paramref name="offset"/>.
        /// </exception>
        public static ulong OffsetBitsUInt64(int offset, int count)
        {
            ThrowHelpers.ThrowForOffset(offset, sizeof(ulong));
            ThrowHelpers.ThrowForCount(count, sizeof(ulong), offset);
            return OffsetBitsUInt64Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="sbyte"/> value where the specified number of bits
        /// starting from the specified low-end bit offset are set. All remaining
        /// bits are unset.
        /// </summary>
        /// <param name="offset">
        /// The number of bits from the low-end at which to start setting bits.
        /// Must not exceed the number of available bits in a <see cref="sbyte"/> value.
        /// </param>
        /// <param name="count">
        /// The number of bits to set.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="sbyte"/> value with <paramref name="count"/> bits set,
        /// starting with the lowest bits set at the 0-based index indicated by
        /// <paramref name="offset"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number of
        /// bits in a <see cref="sbyte"/> value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is negative or greater than the difference
        /// between the number of bits in a <see cref="sbyte"/> value and
        /// <paramref name="offset"/>.
        /// </exception>
        public static sbyte OffsetBitsInt8(int offset, int count)
        {
            ThrowHelpers.ThrowForOffset(offset, sizeof(sbyte));
            ThrowHelpers.ThrowForCount(count, sizeof(sbyte), offset);
            return OffsetBitsInt8Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="short"/> value where the specified number of bits
        /// starting from the specified low-end bit offset are set. All remaining
        /// bits are unset.
        /// </summary>
        /// <param name="offset">
        /// The number of bits from the low-end at which to start setting bits.
        /// Must not exceed the number of available bits in a <see cref="short"/> value.
        /// </param>
        /// <param name="count">
        /// The number of bits to set.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="short"/> value with <paramref name="count"/> bits set,
        /// starting with the lowest bits set at the 0-based index indicated by
        /// <paramref name="offset"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number of
        /// bits in a <see cref="short"/> value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is negative or greater than the difference
        /// between the number of bits in a <see cref="short"/> value and
        /// <paramref name="offset"/>.
        /// </exception>
        public static short OffsetBitsInt16(int offset, int count)
        {
            ThrowHelpers.ThrowForOffset(offset, sizeof(short));
            ThrowHelpers.ThrowForCount(count, sizeof(short), offset);
            return OffsetBitsInt16Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="int"/> value where the specified number of bits
        /// starting from the specified low-end bit offset are set. All remaining
        /// bits are unset.
        /// </summary>
        /// <param name="offset">
        /// The number of bits from the low-end at which to start setting bits.
        /// Must not exceed the number of available bits in a <see cref="int"/> value.
        /// </param>
        /// <param name="count">
        /// The number of bits to set.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="int"/> value with <paramref name="count"/> bits set,
        /// starting with the lowest bits set at the 0-based index indicated by
        /// <paramref name="offset"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number of
        /// bits in a <see cref="int"/> value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is negative or greater than the difference
        /// between the number of bits in a <see cref="int"/> value and
        /// <paramref name="offset"/>.
        /// </exception>
        public static int OffsetBitsInt32(int offset, int count)
        {
            ThrowHelpers.ThrowForOffset(offset, sizeof(int));
            ThrowHelpers.ThrowForCount(count, sizeof(int), offset);
            return OffsetBitsInt32Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="long"/> value where the specified number of bits
        /// starting from the specified low-end bit offset are set. All remaining
        /// bits are unset.
        /// </summary>
        /// <param name="offset">
        /// The number of bits from the low-end at which to start setting bits.
        /// Must not exceed the number of available bits in a <see cref="long"/> value.
        /// </param>
        /// <param name="count">
        /// The number of bits to set.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="long"/> value with <paramref name="count"/> bits set,
        /// starting with the lowest bits set at the 0-based index indicated by
        /// <paramref name="offset"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number of
        /// bits in a <see cref="long"/> value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is negative or greater than the difference
        /// between the number of bits in a <see cref="long"/> value and
        /// <paramref name="offset"/>.
        /// </exception>
        public static long OffsetBitsInt64(int offset, int count)
        {
            ThrowHelpers.ThrowForOffset(offset, sizeof(long));
            ThrowHelpers.ThrowForCount(count, sizeof(long), offset);
            return OffsetBitsInt64Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="byte"/> value where all bits starting with bit
        /// at the specified offset bit are set. All remaining lower bits are
        /// unset.
        /// </summary>
        /// <param name="offset">
        /// The 0-based index of the lowest set bit.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="byte"/> value with the higher bits set, starting at the
        /// bit as specified by <paramref name="offset"/>. Lower bits are unset.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number 
        /// </exception>
        public static byte OffsetRemainingUInt8(int offset)
        {
            const int bytes = sizeof(byte);
            ThrowHelpers.ThrowForOffset(offset, bytes);
            const int bits = bytes * 8;
            int count = bits - offset;
            return OffsetBitsUInt8Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="ushort"/> value where all bits starting with bit
        /// at the specified offset bit are set. All remaining lower bits are
        /// unset.
        /// </summary>
        /// <param name="offset">
        /// The 0-based index of the lowest set bit.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="ushort"/> value with the higher bits set, starting at the
        /// bit as specified by <paramref name="offset"/>. Lower bits are unset.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number 
        /// </exception>
        public static ushort OffsetRemainingUInt16(int offset)
        {
            const int bytes = sizeof(ushort);
            ThrowHelpers.ThrowForOffset(offset, bytes);
            const int bits = bytes * 8;
            int count = bits - offset;
            return OffsetBitsUInt16Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="uint"/> value where all bits starting with bit
        /// at the specified offset bit are set. All remaining lower bits are
        /// unset.
        /// </summary>
        /// <param name="offset">
        /// The 0-based index of the lowest set bit.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="uint"/> value with the higher bits set, starting at the
        /// bit as specified by <paramref name="offset"/>. Lower bits are unset.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number 
        /// </exception>
        public static uint OffsetRemainingUInt32(int offset)
        {
            const int bytes = sizeof(uint);
            ThrowHelpers.ThrowForOffset(offset, bytes);
            const int bits = bytes * 8;
            int count = bits - offset;
            return OffsetBitsUInt32Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="ulong"/> value where all bits starting with bit
        /// at the specified offset bit are set. All remaining lower bits are
        /// unset.
        /// </summary>
        /// <param name="offset">
        /// The 0-based index of the lowest set bit.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="ulong"/> value with the higher bits set, starting at the
        /// bit as specified by <paramref name="offset"/>. Lower bits are unset.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number 
        /// </exception>
        public static ulong OffsetRemainingUInt64(int offset)
        {
            const int bytes = sizeof(ulong);
            ThrowHelpers.ThrowForOffset(offset, bytes);
            const int bits = bytes * 8;
            int count = bits - offset;
            return OffsetBitsUInt64Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="sbyte"/> value where all bits starting with bit
        /// at the specified offset bit are set. All remaining lower bits are
        /// unset.
        /// </summary>
        /// <param name="offset">
        /// The 0-based index of the lowest set bit.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="sbyte"/> value with the higher bits set, starting at the
        /// bit as specified by <paramref name="offset"/>. Lower bits are unset.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number 
        /// </exception>
        public static sbyte OffsetRemainingInt8(int offset)
        {
            const int bytes = sizeof(sbyte);
            ThrowHelpers.ThrowForOffset(offset, bytes);
            const int bits = bytes * 8;
            int count = bits - offset;
            return OffsetBitsInt8Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="short"/> value where all bits starting with bit
        /// at the specified offset bit are set. All remaining lower bits are
        /// unset.
        /// </summary>
        /// <param name="offset">
        /// The 0-based index of the lowest set bit.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="short"/> value with the higher bits set, starting at the
        /// bit as specified by <paramref name="offset"/>. Lower bits are unset.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number 
        /// </exception>
        public static short OffsetRemainingInt16(int offset)
        {
            const int bytes = sizeof(short);
            ThrowHelpers.ThrowForOffset(offset, bytes);
            const int bits = bytes * 8;
            int count = bits - offset;
            return OffsetBitsInt16Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="int"/> value where all bits starting with bit
        /// at the specified offset bit are set. All remaining lower bits are
        /// unset.
        /// </summary>
        /// <param name="offset">
        /// The 0-based index of the lowest set bit.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="int"/> value with the higher bits set, starting at the
        /// bit as specified by <paramref name="offset"/>. Lower bits are unset.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number 
        /// </exception>
        public static int OffsetRemainingInt32(int offset)
        {
            const int bytes = sizeof(int);
            ThrowHelpers.ThrowForOffset(offset, bytes);
            const int bits = bytes * 8;
            int count = bits - offset;
            return OffsetBitsInt32Unguarded(offset, count);
        }

        /// <summary>
        /// Returns a <see cref="long"/> value where all bits starting with bit
        /// at the specified offset bit are set. All remaining lower bits are
        /// unset.
        /// </summary>
        /// <param name="offset">
        /// The 0-based index of the lowest set bit.
        /// Must not exceed the remaining number of bytes.
        /// </param>
        /// <returns>
        /// A <see cref="long"/> value with the higher bits set, starting at the
        /// bit as specified by <paramref name="offset"/>. Lower bits are unset.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is negative or greater than the number 
        /// </exception>
        public static long OffsetRemainingInt64(int offset)
        {
            const int bytes = sizeof(long);
            ThrowHelpers.ThrowForOffset(offset, bytes);
            const int bits = bytes * 8;
            int count = bits - offset;
            return OffsetBitsInt64Unguarded(offset, count);
        }
    }
}
