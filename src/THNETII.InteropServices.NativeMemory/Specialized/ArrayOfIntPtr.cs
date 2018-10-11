using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory.Specialized
{
    /// <summary>
    /// A typed pointer to an array of <see cref="IntPtr"/> values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types", Justification = nameof(IntPtrOfTExtensions))]
    public struct ArrayOfIntPtr : IArrayPtr<IntPtr>
    {
        /// <summary>
        /// Initializes a new typed pointer with the specified pointer to an unspecified type.
        /// </summary>
        /// <param name="ptr">A pointer to an unspecified type.</param>
        public ArrayOfIntPtr(IntPtr ptr) => Pointer = ptr;

        /// <inheritdoc />
        public IntPtr Pointer { get; }

        /// <summary>
        /// Converts the numeric value of the current pointer to its equivalent
        /// string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance.</returns>
        public override string ToString() => Pointer.ToString();

        /// <summary>
        /// Converts the numeric value of the current pointer to its equivalent
        /// string representation.
        /// </summary>
        /// <param name="format">A format specification that governs how the current pointer is converted.</param>
        /// <returns>The string representation of the value of this instance.</returns>
        public string ToString(string format) => Pointer.ToString(format);
    }
}
