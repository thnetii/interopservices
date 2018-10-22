using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// Represents a typed pointer to a contiguous sequence of <see cref="byte"/> values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types", Justification = nameof(IntPtrOfTExtensions))]
    public struct ByteArrayPtr : IArrayPtr<byte>
    {
        /// <summary>
        /// Initializes a new typed pointer with the specified pointer to an unspecified type.
        /// </summary>
        /// <param name="ptr">A pointer to an unspecified type.</param>
        public ByteArrayPtr(IntPtr ptr) => Pointer = ptr;

        /// <inheritdoc />
        public IntPtr Pointer { get; }
    }
}
