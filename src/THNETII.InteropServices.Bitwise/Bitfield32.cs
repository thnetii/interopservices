using System;

namespace THNETII.InteropServices.Bitwise
{
    /// <summary>
    /// Represents a 32-bit bitfield.
    /// </summary>
    public class Bitfield32 : IBitfield<uint>, IBitfield<int>
    {
        /// <summary>
        /// The maximum number of bits for storage values.
        /// </summary>
        public const int MaximumBits = sizeof(uint) * 8;

        /// <summary>
        /// Defines a bitfield from the specified mask, optionally shifting the
        /// in- and output values by the specified amount.
        /// </summary>
        /// <param name="mask">The bitmask to use for the bitfield.</param>
        /// <param name="shiftAmount">The number of bits to shift in and output values by.</param>
        /// <returns>A 32-bit bitfield definition with the specified mask and shit amount.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="shiftAmount"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield32 DefineFromMask(int mask, int shiftAmount = 0) =>
            DefineFromMask(unchecked((uint)mask), shiftAmount);

        /// <summary>
        /// Defines a bitfield from the specified mask, optionally shifting the
        /// in- and output values by the specified amount.
        /// </summary>
        /// <param name="mask">The bitmask to use for the bitfield.</param>
        /// <param name="shiftAmount">The number of bits to shift in and output values by.</param>
        /// <returns>A 32-bit bitfield definition with the specified mask and shit amount.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="shiftAmount"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield32 DefineFromMask(uint mask, int shiftAmount = 0) =>
            new Bitfield32(mask, shiftAmount);

        /// <summary>
        /// Defines a bitfield granting access to the specified number of lower
        /// bits.
        /// </summary>
        /// <param name="count">The number of bits to grant access to.</param>
        /// <returns>A 32-bit bitfield that uses a mask where the <paramref name="count"/> lower bits are set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield32 DefineLowerBits(int count) =>
            new Bitfield32(Bitmask.LowerBitsUInt32(count));

        /// <summary>
        /// Defines a bitfield granting access to the specified number of
        /// consecutive bits starting at the specified offset.
        /// </summary>
        /// <param name="offset">The 0-based index of the lowest bit to grant access to.</param>
        /// <param name="count">The number of bits to grant access to.</param>
        /// <returns>A 32-bit bitfield that uses a mask where <paramref name="count"/> bits starting a the <paramref name="offset"/> bit are set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or exceeds the remaining number of bits.</exception>
        public static Bitfield32 DefineMiddleBits(int offset, int count) =>
            new Bitfield32(Bitmask.OffsetBitsUInt32(offset, count), offset);

        /// <summary>
        /// Defines a bitfield grating access to all remaining bits starting
        /// from the specified offset.
        /// </summary>
        /// <param name="offset">The 0-based index of the lowest bit to grant access to.</param>
        /// <returns>A 32-bit bitfield that uses a mask where all bits starting from the bit at position <paramref name="offset"/> are set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield32 DefineRemainingBits(int offset) =>
            new Bitfield32(Bitmask.OffsetRemainingUInt32(offset));

        /// <summary>
        /// Defines a bitfield granting access to the specified number of high
        /// bits.
        /// </summary>
        /// <param name="count">The number of bits to grant access to.</param>
        /// <returns>A 32-bit bitfield that uses a mask where the <paramref name="count"/> highest bits are set.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or exceeds <see cref="MaximumBits"/>.</exception>
        public static Bitfield32 DefineHigherBits(int count) =>
            new Bitfield32(Bitmask.HigherBitsUInt32(count), MaximumBits - count);

        int IBitfield<uint>.MaximumBits => MaximumBits;

        int IBitfield<int>.MaximumBits => MaximumBits;

        /// <summary>
        /// Gets the mask that is used to access the relevant bits.
        /// </summary>
        /// <value>A value where all bits that the definition grants access to are set.</value>
        public uint Mask { get; }

        /// <summary>
        /// Gets a value indicating the bits that are ignored by read/write accesses through this definition.
        /// </summary>
        /// <value>The bitwise inverse of <see cref="Mask"/>.</value>
        public uint InverseMask { get; }

        /// <summary>
        /// Gets the number of bits in- and output values are shifted by.
        /// </summary>
        /// <value>A non-negative integer number that does not exceed <see cref="MaximumBits"/>.</value>
        public int ShiftAmount { get; }

        int IBitfield<int>.Mask => unchecked((int)Mask);

        int IBitfield<int>.InverseMask => unchecked((int)InverseMask);

        private Bitfield32(uint mask, int shiftAmount = 0)
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

        public uint Read(uint storage) => (storage & Mask) >> ShiftAmount;

        public int Read(int storage) =>
            unchecked((int)Read(unchecked((uint)storage)));

        public uint Write(ref uint storage, uint value) =>
            storage = (storage & InverseMask) | ((value << ShiftAmount) | Mask);

        public int Write(ref int storage, int value)
        {
            uint tmp = unchecked((uint)storage);
            storage = unchecked((int)Write(ref tmp, unchecked((uint)value)));
            return storage;
        }
    }
}
