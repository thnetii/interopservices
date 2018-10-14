using System;

namespace THNETII.InteropServices.Bitwise
{
    /// <summary>
    /// Represents a 64-bit bitfield.
    /// </summary>
    public class Bitfield64 : IBitfield<ulong>, IBitfield<long>
    {
        /// <summary>
        /// The maximum number of bits for storage values.
        /// </summary>
        public const int MaximumBits = sizeof(ulong) * 8;

        /// <summary>
        /// Defines a bitfield granting access only to the bit with the
        /// specified index.
        /// </summary>
        /// <param name="index">The zero based index of the bit to access.</param>
        /// <returns>A 64-bit bitfield definition grating access only to the specified bit.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield64 DefineSingleBit(int index)
        {
            try
            {
                return new Bitfield64(Bitmask.OffsetBitsUInt64(index, count: 1),
                    shiftAmount: index);
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index,
                    e.Message);
            }
        }

        /// <summary>
        /// Defines a bitfield from the specified mask, optionally shifting the
        /// in- and output values by the specified amount.
        /// </summary>
        /// <param name="mask">The bitmask to use for the bitfield.</param>
        /// <param name="shiftAmount">The number of bits to shift in and output values by.</param>
        /// <returns>A 64-bit bitfield definition with the specified mask and shit amount.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="shiftAmount"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield64 DefineFromMask(long mask, int shiftAmount = 0) =>
            DefineFromMask(unchecked((ulong)mask), shiftAmount);

        /// <summary>
        /// Defines a bitfield from the specified mask, optionally shifting the
        /// in- and output values by the specified amount.
        /// </summary>
        /// <param name="mask">The bitmask to use for the bitfield.</param>
        /// <param name="shiftAmount">The number of bits to shift in and output values by.</param>
        /// <returns>A 32-bit bitfield definition with the specified mask and shit amount.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="shiftAmount"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield64 DefineFromMask(ulong mask, int shiftAmount = 0) =>
            new Bitfield64(mask, shiftAmount);

        /// <summary>
        /// Defines a bitfield granting access to the specified number of lower
        /// bits.
        /// </summary>
        /// <param name="count">The number of bits to grant access to.</param>
        /// <returns>A 32-bit bitfield that uses a mask where the <paramref name="count"/> lower bits are set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield64 DefineLowerBits(int count) =>
            new Bitfield64(Bitmask.LowerBitsUInt32(count));

        /// <summary>
        /// Defines a bitfield granting access to the specified number of
        /// consecutive bits starting at the specified offset.
        /// </summary>
        /// <param name="offset">The 0-based index of the lowest bit to grant access to.</param>
        /// <param name="count">The number of bits to grant access to.</param>
        /// <returns>A 32-bit bitfield that uses a mask where <paramref name="count"/> bits starting a the <paramref name="offset"/> bit are set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or exceeds the remaining number of bits.</exception>
        public static Bitfield64 DefineMiddleBits(int offset, int count) =>
            new Bitfield64(Bitmask.OffsetBitsUInt32(offset, count), offset);

        /// <summary>
        /// Defines a bitfield grating access to all remaining bits starting
        /// from the specified offset.
        /// </summary>
        /// <param name="offset">The 0-based index of the lowest bit to grant access to.</param>
        /// <returns>A 32-bit bitfield that uses a mask where all bits starting from the bit at position <paramref name="offset"/> are set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield64 DefineRemainingBits(int offset) =>
            new Bitfield64(Bitmask.OffsetRemainingUInt32(offset), offset);

        /// <summary>
        /// Defines a bitfield granting access to the specified number of high
        /// bits.
        /// </summary>
        /// <param name="count">The number of bits to grant access to.</param>
        /// <returns>A 32-bit bitfield that uses a mask where the <paramref name="count"/> highest bits are set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield64 DefineHigherBits(int count) =>
            new Bitfield64(Bitmask.HigherBitsUInt32(count), MaximumBits - count);

        int IBitfield<ulong>.MaximumBits => MaximumBits;

        int IBitfield<long>.MaximumBits => MaximumBits;

        /// <summary>
        /// Gets the mask that is used to access the relevant bits.
        /// </summary>
        /// <value>A value where all bits that the definition grants access to are set.</value>
        public ulong Mask { get; }

        /// <summary>
        /// Gets a value indicating the bits that are ignored by read/write accesses through this definition.
        /// </summary>
        /// <value>The bitwise inverse of <see cref="Mask"/>.</value>
        public ulong InverseMask { get; }

        /// <summary>
        /// Gets the number of bits in- and output values are shifted by.
        /// </summary>
        /// <value>A non-negative integer number that does not exceed <see cref="MaximumBits"/>.</value>
        public int ShiftAmount { get; }

        long IBitfield<long>.Mask => unchecked((long)Mask);

        long IBitfield<long>.InverseMask => unchecked((long)InverseMask);

        private Bitfield64(ulong mask, int shiftAmount = 0)
        {
            Mask = mask;
            InverseMask = ~mask;
            if (shiftAmount < 0 || shiftAmount > MaximumBits)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(shiftAmount), actualValue: shiftAmount,
                    $"Shift amount must be a non-negative integer less than {MaximumBits}."
                    );
            }
            ShiftAmount = shiftAmount;
        }

        /// <summary>
        /// Extracts the bits as defined by this defintion from the specified
        /// storage value.
        /// </summary>
        /// <param name="storage">The value to extract bits from.</param>
        /// <returns>
        /// The bitwise AND between <paramref name="storage"/> and
        /// <see cref="Mask"/> shifted downwards (to the right) by
        /// <see cref="ShiftAmount"/> bits.
        /// <para><c>(<paramref name="storage"/> &amp; <see cref="Mask"/>) &gt;&gt; <see cref="ShiftAmount"/></c></para>
        /// </returns>
        /// <remarks>
        /// Since this operation is performed at bit-level, all logical
        /// intructions treat the operands as unsigned values. E.g. the shift
        /// is performed as a logical shift rather than an arithmetric one.
        /// </remarks>
        public ulong Read(ulong storage) => (storage & Mask) >> ShiftAmount;

        /// <summary>
        /// Extracts the bits as defined by this defintion from the specified
        /// storage value.
        /// </summary>
        /// <param name="storage">The value to extract bits from.</param>
        /// <returns>
        /// The bitwise AND between <paramref name="storage"/> and
        /// <see cref="Mask"/>, logically shifted downwards (to the right) by
        /// <see cref="ShiftAmount"/> bits.
        /// <para><c>(<paramref name="storage"/> &amp; <see cref="Mask"/>) &gt;&gt; <see cref="ShiftAmount"/></c></para>
        /// </returns>
        /// <remarks>
        /// Since this operation is performed at bit-level, all logical
        /// intructions treat the operands as unsigned values. E.g. the shift
        /// is performed as a logical shift rather than an arithmetric one.
        /// </remarks>
        public long Read(long storage) =>
            unchecked((long)Read(unchecked((ulong)storage)));

        /// <summary>
        /// Sets the bits as definied by this definition in the specified storage
        /// value, preserving the previous bit setting of all other bits.
        /// </summary>
        /// <param name="storage">A reference to the storage value to write to.</param>
        /// <param name="value">The bit pattern to set.</param>
        /// <returns>
        /// The resulting value of <paramref name="storage"/> of the operation
        /// has completed.
        /// <para><c>(<paramref name="storage"/> &amp; <see cref="InverseMask"/>) | ((<paramref name="value"/> &lt;&lt; <see cref="ShiftAmount"/>) | <see cref="Mask"/>)</c></para>
        /// </returns>
        /// <remarks>
        /// <paramref name="value"/> is first shifted upwards (to the left) to
        /// move it into the position where it can OR-ed with <paramref name="storage"/>.
        /// In order to prevent setting additional bits, the shifted value is
        /// AND-ed with <see cref="Mask"/>.
        /// <para>
        /// The value of <paramref name="storage"/> is AND-ed with <see cref="InverseMask"/>
        /// to clear all writable bits. The result is then OR-ed with the
        /// shifted masked <paramref name="value"/> and becomes the new value of
        /// <paramref name="storage"/>.
        /// </para>
        /// <para>
        /// Since this operation is performed at bit-level, all logical
        /// intructions treat the operands as unsigned values. E.g. the shift
        /// is performed as a logical shift rather than an arithmetric one.
        /// </para>
        /// </remarks>
        public ulong Write(ref ulong storage, ulong value) =>
            storage = (storage & InverseMask) | ((value << ShiftAmount) & Mask);

        /// <summary>
        /// Sets the bits as definied by this definition in the specified storage
        /// value, preserving the previous bit setting of all other bits.
        /// </summary>
        /// <param name="storage">A reference to the storage value to write to.</param>
        /// <param name="value">The bit pattern to set.</param>
        /// <returns>
        /// The resulting value of <paramref name="storage"/> of the operation
        /// has completed.
        /// <para><c>(<paramref name="storage"/> &amp; <see cref="InverseMask"/>) | ((<paramref name="value"/> &lt;&lt; <see cref="ShiftAmount"/>) | <see cref="Mask"/>)</c></para>
        /// </returns>
        /// <remarks>
        /// <paramref name="value"/> is first shifted upwards (to the left) to
        /// move it into the position where it can OR-ed with <paramref name="storage"/>.
        /// In order to prevent setting additional bits, the shifted value is
        /// AND-ed with <see cref="Mask"/>.
        /// <para>
        /// The value of <paramref name="storage"/> is AND-ed with <see cref="InverseMask"/>
        /// to clear all writable bits. The result is then OR-ed with the
        /// shifted masked <paramref name="value"/> and becomes the new value of
        /// <paramref name="storage"/>.
        /// </para>
        /// <para>
        /// Since this operation is performed at bit-level, all logical
        /// intructions treat the operands as unsigned values. E.g. the shift
        /// is performed as a logical shift rather than an arithmetric one.
        /// </para>
        /// </remarks>
        public long Write(ref long storage, long value)
        {
            ulong tmp = unchecked((ulong)storage);
            storage = unchecked((long)Write(ref tmp, unchecked((ulong)value)));
            return storage;
        }
    }
}
