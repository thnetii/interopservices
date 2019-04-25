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
            internal static void ThrowForCount(int count, int bitSize, int offset = 0)
            {
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(count), actualValue: count,
                        "Number of bits must be a non-negative integer."
                        );
                }
                int bits = bitSize - offset;
                if (count > bits)
                {
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(count), actualValue: count,
                        $"Number of bits must not exceed the maximum number of available bits ({bits} bits)."
                        );
                }
            }

            internal static void ThrowForOffset(int offset, int bitSize)
            {
                if (offset < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(offset), actualValue: offset,
                        "Bit offset must be a non-negative integer."
                        );
                }
                if (offset > bitSize)
                {
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(offset), actualValue: offset,
                        $"Bit offset must be less than the number of available bits ({bitSize} bits)."
                        );
                }
            }
        }

        private static uint LowerBitsUnguarded(uint allSet, int bitSize, int count) =>
            count > 0 ? allSet >> (bitSize - count) : 0;

        private static ulong LowerBitsUnguarded(ulong allSet, int bitSize, int count) =>
            count > 0 ? allSet >> (bitSize - count) : 0;

        private static uint HigherBitsUnguarded(uint allSet, int bitSize, int count) =>
            count > 0 ? allSet << (bitSize - count) : 0;

        private static ulong HigherBitsUnguarded(ulong allSet, int bitSize, int count) =>
            count > 0 ? allSet << (bitSize - count) : 0;

        private static uint OffsetRemainingUnguarded(uint allSet, int bitSize, int offset) =>
            bitSize > offset ? allSet << offset : 0;

        private static ulong OffsetRemainingUnguarded(ulong allSet, int bitSize, int offset) =>
            bitSize > offset ? allSet << offset : 0;

        /// <summary>
        /// Returns a <see cref="byte"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="byte"/> value.</param>
        /// <returns>A <see cref="byte"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="byte"/> value.</exception>
        public static byte LowerBitsUInt8(int count)
        {
            const uint allSet = unchecked((byte)~0U);
            const int maxBits = sizeof(byte) * 8;
            ThrowHelpers.ThrowForCount(count, maxBits);
            return (byte)LowerBitsUnguarded(allSet, maxBits, count);
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
            const uint allSet = unchecked((ushort)~0U);
            const int maxBits = sizeof(ushort) * 8;
            ThrowHelpers.ThrowForCount(count, maxBits);
            return (ushort)LowerBitsUnguarded(allSet, maxBits, count);
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
            const uint allSet = ~0U;
            const int maxBits = sizeof(uint) * 8;
            ThrowHelpers.ThrowForCount(count, maxBits);
            return LowerBitsUnguarded(allSet, maxBits, count);
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
            const ulong allSet = ~0UL;
            const int maxBits = sizeof(ulong) * 8;
            ThrowHelpers.ThrowForCount(count, maxBits);
            return LowerBitsUnguarded(allSet, maxBits, count);
        }

        /// <summary>
        /// Returns a <see cref="sbyte"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="sbyte"/> value.</param>
        /// <returns>A <see cref="sbyte"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="sbyte"/> value.</exception>
        public static sbyte LowerBitsInt8(int count) =>
            (sbyte)LowerBitsUInt8(count);

        /// <summary>
        /// Returns a <see cref="short"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="short"/> value.</param>
        /// <returns>A <see cref="short"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="short"/> value.</exception>
        public static short LowerBitsInt16(int count) =>
            (short)LowerBitsUInt16(count);

        /// <summary>
        /// Returns a <see cref="int"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="int"/> value.</param>
        /// <returns>A <see cref="int"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="int"/> value.</exception>
        public static int LowerBitsInt32(int count) =>
            (int)LowerBitsUInt32(count);

        /// <summary>
        /// Returns a <see cref="long"/> value where the specified number of the
        /// lower bits are set. The remaining higher bits are unset.
        /// </summary>
        /// <param name="count">The number of low bits to set. Must not exceed the number of available bits in a <see cref="long"/> value.</param>
        /// <returns>A <see cref="long"/> with the lower <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="long"/> value.</exception>
        public static long LowerBitsInt64(int count) =>
            (long)LowerBitsUInt64(count);

        /// <summary>
        /// Returns a <see cref="byte"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="byte"/> value.</param>
        /// <returns>A <see cref="byte"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="byte"/> value.</exception>
        public static byte HigherBitsUInt8(int count)
        {
            const uint allSet = unchecked((byte)~0U);
            const int maxBits = sizeof(byte) * 8;
            ThrowHelpers.ThrowForCount(count, maxBits);
            return (byte)HigherBitsUnguarded(allSet, maxBits, count);
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
            const uint allSet = unchecked((ushort)~0U);
            const int maxBits = sizeof(ushort) * 8;
            ThrowHelpers.ThrowForCount(count, maxBits);
            return (ushort)HigherBitsUnguarded(allSet, maxBits, count);
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
            const uint allSet = ~0U;
            const int maxBits = sizeof(uint) * 8;
            ThrowHelpers.ThrowForCount(count, maxBits);
            return HigherBitsUnguarded(allSet, maxBits, count);
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
            const ulong allSet = ~0UL;
            const int maxBits = sizeof(ulong) * 8;
            ThrowHelpers.ThrowForCount(count, maxBits);
            return HigherBitsUnguarded(allSet, maxBits, count);
        }

        /// <summary>
        /// Returns a <see cref="sbyte"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="sbyte"/> value.</param>
        /// <returns>A <see cref="sbyte"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="sbyte"/> value.</exception>
        public static sbyte HigherBitsInt8(int count) =>
            (sbyte)HigherBitsUInt8(count);

        /// <summary>
        /// Returns a <see cref="short"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="short"/> value.</param>
        /// <returns>A <see cref="short"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="short"/> value.</exception>
        public static short HigherBitsInt16(int count) =>
            (short)HigherBitsUInt16(count);

        /// <summary>
        /// Returns a <see cref="int"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="int"/> value.</param>
        /// <returns>A <see cref="int"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="int"/> value.</exception>
        public static int HigherBitsInt32(int count) =>
            (int)HigherBitsUInt32(count);

        /// <summary>
        /// Returns a <see cref="long"/> value where the specified number of the
        /// higher bits are set. The remaining lower bits are unset.
        /// </summary>
        /// <param name="count">The number of high bits to set. Must not exceed the number of available bits in a <see cref="long"/> value.</param>
        /// <returns>A <see cref="long"/> with the higher <paramref name="count"/> bits set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or greater than the number of bits in a <see cref="long"/> value.</exception>
        public static long HigherBitsInt64(int count) =>
            (long)HigherBitsUInt64(count);

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
            const uint allSet = unchecked((byte)~0U);
            const int bitSize = sizeof(byte) * 8;
            ThrowHelpers.ThrowForOffset(offset, bitSize);
            ThrowHelpers.ThrowForCount(count, bitSize, offset);
            return (byte)(
                OffsetRemainingUnguarded(allSet, bitSize, offset)
                &
                LowerBitsUnguarded(allSet, bitSize, offset + count)
                );
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
            const uint allSet = unchecked((ushort)~0U);
            const int bitSize = sizeof(ushort) * 8;
            ThrowHelpers.ThrowForOffset(offset, bitSize);
            ThrowHelpers.ThrowForCount(count, bitSize, offset);
            return (ushort)(
                OffsetRemainingUnguarded(allSet, bitSize, offset)
                &
                LowerBitsUnguarded(allSet, bitSize, offset + count)
                );
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
            const uint allSet = ~0U;
            const int bitSize = sizeof(uint) * 8;
            ThrowHelpers.ThrowForOffset(offset, bitSize);
            ThrowHelpers.ThrowForCount(count, bitSize, offset);
            return (
                OffsetRemainingUnguarded(allSet, bitSize, offset)
                &
                LowerBitsUnguarded(allSet, bitSize, offset + count)
                );
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
            const ulong allSet = ~0UL;
            const int bitSize = sizeof(ulong) * 8;
            ThrowHelpers.ThrowForOffset(offset, bitSize);
            ThrowHelpers.ThrowForCount(count, bitSize, offset);
            return (
                OffsetRemainingUnguarded(allSet, bitSize, offset)
                &
                LowerBitsUnguarded(allSet, bitSize, offset + count)
                );
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
        public static sbyte OffsetBitsInt8(int offset, int count) =>
            (sbyte)OffsetBitsUInt8(offset, count);

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
        public static short OffsetBitsInt16(int offset, int count) =>
            (short)OffsetBitsUInt16(offset, count);

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
        public static int OffsetBitsInt32(int offset, int count) =>
            (int)OffsetBitsUInt32(offset, count);

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
        public static long OffsetBitsInt64(int offset, int count) =>
            (long)OffsetBitsUInt64(offset, count);

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
            const uint allSet = unchecked((byte)~0U);
            const int bitSize = sizeof(byte) * 8;
            ThrowHelpers.ThrowForOffset(offset, bitSize);
            return (byte)OffsetRemainingUnguarded(allSet, bitSize, offset);
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
            const uint allSet = unchecked((ushort)~0U);
            const int bitSize = sizeof(ushort) * 8;
            ThrowHelpers.ThrowForOffset(offset, bitSize);
            return (ushort)OffsetRemainingUnguarded(allSet, bitSize, offset);
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
            const uint allSet = ~0U;
            const int bitSize = sizeof(uint) * 8;
            ThrowHelpers.ThrowForOffset(offset, bitSize);
            return OffsetRemainingUnguarded(allSet, bitSize, offset);
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
            const ulong allSet = ~0UL;
            const int bitSize = sizeof(ulong) * 8;
            ThrowHelpers.ThrowForOffset(offset, bitSize);
            return OffsetRemainingUnguarded(allSet, bitSize, offset);
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
        public static sbyte OffsetRemainingInt8(int offset) =>
            (sbyte)OffsetRemainingUInt8(offset);

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
        public static short OffsetRemainingInt16(int offset) =>
            (short)OffsetRemainingUInt16(offset);

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
        public static int OffsetRemainingInt32(int offset) =>
            (int)OffsetRemainingUInt32(offset);

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
        public static long OffsetRemainingInt64(int offset) =>
            (long)OffsetRemainingUInt64(offset);
    }
}
