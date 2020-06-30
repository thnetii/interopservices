using System;

namespace THNETII.InteropServices.Bitwise
{
    /// <summary>
    /// Represents a 16-bit bitfield.
    /// </summary>
    public class Bitfield16 : IBitfield<ushort>, IBitfield<short>
    {
        /// <summary>
        /// The maximum number of bits for storage values.
        /// </summary>
        public const int MaximumBits = sizeof(ushort) * 8;

        /// <summary>
        /// Defines a bitfield granting access only to the bit with the
        /// specified index.
        /// </summary>
        /// <param name="index">The zero based index of the bit to access.</param>
        /// <returns>A 16-bit bitfield definition grating access only to the specified bit.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield16 Bit(int index)
        {
            try
            {
                return new Bitfield16(Bitmask.OffsetBitsUInt16(index, count: 1),
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
        /// <returns>A 16-bit bitfield definition with the specified mask and shit amount.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="shiftAmount"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield16 FromMask(short mask, int shiftAmount = 0) =>
            FromMask(unchecked((ushort)mask), shiftAmount);

        /// <summary>
        /// Defines a bitfield from the specified mask, optionally shifting the
        /// in- and output values by the specified amount.
        /// </summary>
        /// <param name="mask">The bitmask to use for the bitfield.</param>
        /// <param name="shiftAmount">The number of bits to shift in and output values by.</param>
        /// <returns>A 16-bit bitfield definition with the specified mask and shit amount.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="shiftAmount"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield16 FromMask(ushort mask, int shiftAmount = 0) =>
            new Bitfield16(mask, shiftAmount);

        /// <summary>
        /// Defines a bitfield granting access to the specified number of lower
        /// bits.
        /// </summary>
        /// <param name="count">The number of bits to grant access to.</param>
        /// <returns>A 16-bit bitfield that uses a mask where the <paramref name="count"/> lower bits are set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield16 LowBits(int count) =>
            new Bitfield16(Bitmask.LowerBitsUInt16(count));

        /// <summary>
        /// Defines a bitfield granting access to the specified number of
        /// consecutive bits starting at the specified offset.
        /// </summary>
        /// <param name="offset">The 0-based index of the lowest bit to grant access to.</param>
        /// <param name="count">The number of bits to grant access to.</param>
        /// <returns>A 16-bit bitfield that uses a mask where <paramref name="count"/> bits starting a the <paramref name="offset"/> bit are set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or exceeds the remaining number of bits.</exception>
        public static Bitfield16 SelectBits(int offset, int count) =>
            new Bitfield16(Bitmask.OffsetBitsUInt16(offset, count), offset);

        /// <summary>
        /// Defines a bitfield grating access to all remaining bits starting
        /// from the specified offset.
        /// </summary>
        /// <param name="offset">The 0-based index of the lowest bit to grant access to.</param>
        /// <returns>A 16-bit bitfield that uses a mask where all bits starting from the bit at position <paramref name="offset"/> are set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield16 RemainingBits(int offset) =>
            new Bitfield16(Bitmask.OffsetRemainingUInt16(offset), offset);

        /// <summary>
        /// Defines a bitfield granting access to the specified number of high
        /// bits.
        /// </summary>
        /// <param name="count">The number of bits to grant access to.</param>
        /// <returns>A 16-bit bitfield that uses a mask where the <paramref name="count"/> highest bits are set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield16 HighBits(int count) =>
            new Bitfield16(Bitmask.HigherBitsUInt16(count), MaximumBits - count);

        int IBitfield<ushort>.MaximumBits => MaximumBits;

        int IBitfield<short>.MaximumBits => MaximumBits;

        /// <summary>
        /// Gets the mask that is used to access the relevant bits.
        /// </summary>
        /// <value>A value where all bits that the definition grants access to are set.</value>
        public ushort Mask { get; }

        /// <summary>
        /// Gets a value indicating the bits that are ignored by read/write accesses through this definition.
        /// </summary>
        /// <value>The bitwise inverse of <see cref="Mask"/>.</value>
        public ushort InverseMask { get; }

        /// <summary>
        /// Gets the number of bits in- and output values are shifted by.
        /// </summary>
        /// <value>A non-negative integer number that does not exceed <see cref="MaximumBits"/>.</value>
        public int ShiftAmount { get; }

        short IBitfield<short>.Mask => unchecked((short)Mask);

        short IBitfield<short>.InverseMask => unchecked((short)InverseMask);

        private Bitfield16(ushort mask, int shiftAmount = 0)
        {
            Mask = mask;
            InverseMask = (ushort)~mask;
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
        public ushort Read(ushort storage) => (ushort)((uint)ReadMasked(storage) >> ShiftAmount);

        /// <summary>
        /// Extracts the bits as defined by this defintion from the specified
        /// storage value without shifting.
        /// </summary>
        /// <param name="storage">The value to extract bits from.</param>
        /// <returns>
        /// The bitwise AND between <paramref name="storage"/> and
        /// <see cref="Mask"/>.
        /// <para><c><paramref name="storage"/> &amp; <see cref="Mask"/></c></para>
        /// </returns>
        public ushort ReadMasked(ushort storage) => (ushort)((uint)storage & Mask);

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
        public short Read(short storage) =>
            unchecked((short)Read(unchecked((ushort)storage)));

        /// <summary>
        /// Extracts the bits as defined by this defintion from the specified
        /// storage value without shifting.
        /// </summary>
        /// <param name="storage">The value to extract bits from.</param>
        /// <returns>
        /// The bitwise AND between <paramref name="storage"/> and
        /// <see cref="Mask"/>.
        /// <para><c><paramref name="storage"/> &amp; <see cref="Mask"/></c></para>
        /// </returns>
        public short ReadMasked(short storage) =>
            unchecked((short)ReadMasked(unchecked((ushort)storage)));

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
        public ushort Write(ref ushort storage, ushort value) =>
            WriteMasked(ref storage, (ushort)((uint)value << ShiftAmount));

        /// <summary>
        /// Sets the bits as definied by this definition in the specified storage
        /// value, preserving the previous bit setting of all other bits.
        /// </summary>
        /// <param name="storage">A reference to the storage value to write to.</param>
        /// <param name="value">The bit pattern to set.</param>
        /// <returns>
        /// The resulting value of <paramref name="storage"/> of the operation
        /// has completed.
        /// <para><c>(<paramref name="storage"/> &amp; <see cref="InverseMask"/>) | (<paramref name="value"/> | <see cref="Mask"/>)</c></para>
        /// </returns>
        /// <remarks>
        /// In order to prevent setting additional bits, <paramref name="value"/>
        /// is AND-ed with <see cref="Mask"/>.
        /// <para>
        /// The value of <paramref name="storage"/> is AND-ed with <see cref="InverseMask"/>
        /// to clear all writable bits. The result is then OR-ed with the
        /// masked <paramref name="value"/> and becomes the new value of
        /// <paramref name="storage"/>.
        /// </para>
        /// </remarks>
        public ushort WriteMasked(ref ushort storage, ushort value) =>
            storage = (ushort)(((uint)storage & InverseMask) | ((uint)value & Mask));

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
        public short Write(ref short storage, short value) =>
            WriteMasked(ref storage, (short)((int)value << ShiftAmount));

        /// <summary>
        /// Sets the bits as definied by this definition in the specified storage
        /// value, preserving the previous bit setting of all other bits.
        /// </summary>
        /// <param name="storage">A reference to the storage value to write to.</param>
        /// <param name="value">The bit pattern to set.</param>
        /// <returns>
        /// The resulting value of <paramref name="storage"/> of the operation
        /// has completed.
        /// <para><c>(<paramref name="storage"/> &amp; <see cref="InverseMask"/>) | (<paramref name="value"/> | <see cref="Mask"/>)</c></para>
        /// </returns>
        /// <remarks>
        /// In order to prevent setting additional bits, <paramref name="value"/>
        /// is AND-ed with <see cref="Mask"/>.
        /// <para>
        /// The value of <paramref name="storage"/> is AND-ed with <see cref="InverseMask"/>
        /// to clear all writable bits. The result is then OR-ed with the
        /// masked <paramref name="value"/> and becomes the new value of
        /// <paramref name="storage"/>.
        /// </para>
        /// </remarks>
        public short WriteMasked(ref short storage, short value) =>
            storage = (short)((storage & (short)InverseMask) | (value & (short)Mask));
    }
}
