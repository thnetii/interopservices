namespace THNETII.InteropServices.Bitwise
{
    /// <summary>
    /// Specifies a bitfield definition for a specific underlying storage type.
    /// </summary>
    /// <typeparam name="T">
    /// The storage type for which the bitfield grants access to the individual
    /// bits.
    /// </typeparam>
    public interface IBitfield<T>
    {
        /// <summary>
        /// Gets the total number of bits in values of type <typeparamref name="T"/>.
        /// This is usually a constant value.
        /// </summary>
        int MaximumBits { get; }

        /// <summary>
        /// Gets a value indicating which bits can be read from or written to
        /// by this bitfield definition.
        /// </summary>
        T Mask { get; }

        /// <summary>
        /// Gets a value indicating which bits are untouched by write access
        /// through this bitfield definition.
        /// </summary>
        /// <value>The bitwise inverse of <see cref="Mask"/>.</value>
        T InverseMask { get; }

        /// <summary>
        /// Gets the number of bits value are shifted during read and write
        /// accesses through this bitfield definition.
        /// </summary>
        /// <value>A non-negative integer number that does not exceed <see cref="MaximumBits"/>.</value>
        int ShiftAmount { get; }

        /// <summary>
        /// Extracts the relevant bits from the specified storage field value.
        /// </summary>
        /// <param name="storage">The full-sized value from which to extract.</param>
        /// <returns>
        /// The bits in <paramref name="storage"/> masked with <see cref="Mask"/>
        /// and shifted downwards (right) as specified by <see cref="ShiftAmount"/>.
        /// </returns>
        T Read(T storage);

        /// <summary>
        /// Writes the relevant bits to the specified storage location.
        /// </summary>
        /// <param name="storage">A reference to the value to write to.</param>
        /// <param name="value">The value to set, in the bits as defined by this definition.</param>
        /// <returns>
        /// The value of <paramref name="storage"/> after the write completes.
        /// </returns>
        T Write(ref T storage, T value);
    }
}
